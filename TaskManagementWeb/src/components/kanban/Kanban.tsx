import { useState } from 'react';
import type { DragEndEvent } from '@dnd-kit/core';
import { DndContext, DragOverlay, pointerWithin } from '@dnd-kit/core';
import { useKanbanData } from '../../hooks/useKanbanData';
import KanbanColumn from './KanbanColumn';
import KanbanCard from './KanbanCard';
import type { Task, TaskUpdateDTO } from '../../types/task';
import type Label from '../../types/label';
import { toast } from 'react-toastify';
import type { UseMutateAsyncFunction } from '@tanstack/react-query';
import { ClipLoader } from 'react-spinners';

interface KanbanProps {
    tasks: Task[] | undefined;
    labels: Label[] | undefined;
    isLoading: boolean;
    updateTask: UseMutateAsyncFunction<any, Error, TaskUpdateDTO, unknown>;
    updatePriority: UseMutateAsyncFunction<any, Error, {
        taskId: string;
        newPriority: string;
    }, unknown>;
    updateLabel: UseMutateAsyncFunction<any, Error, {
        taskId: string;
        newLabelId: string;
    }, unknown>;
}

const Kanban = ({ tasks, labels, isLoading, updateTask, updatePriority, updateLabel }: KanbanProps) => {
    const { board, updateTaskPosition, discardAllChanges, getChangedTasks, hasChanges } = useKanbanData({ tasks, labels, isLoading });
    const [activeId, setActiveId] = useState<string | null>(null);
    const [activeDragTask, setActiveDragTask] = useState<Task | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleDragStart = (event: any) => {
        const { active } = event;
        setActiveId(active.id);

        const draggedTask = tasks?.find(t => t.id === active.id);
        setActiveDragTask(draggedTask || null);
    };

    const handleDragEnd = async (event: DragEndEvent) => {
        const { active, over } = event;
        setActiveId(null);

        if (!over || !activeDragTask) {
            setActiveDragTask(null);
            return;
        }

        const overId = over.id as string;
        const [targetPriority, targetLabelId] = overId.split('/');

        if (!targetPriority || !targetLabelId) {
            console.error('Geçersiz hedef:', overId);
            setActiveDragTask(null);
            return;
        }

        updateTaskPosition(activeDragTask.id, targetPriority, targetLabelId);
        setActiveDragTask(null);
    };

    const handleConfirmChanges = async () => {
        const changedTasks = getChangedTasks();

        if (changedTasks.length === 0) {
            toast.info('Değişiklik yapılmadı');
            return;
        }

        setIsSubmitting(true);

        try {
            await Promise.all(
                changedTasks.map(async changed => {
                    const { changedTask, changes } = changed;

                    if (changes.labelChanged && changes.priorityChanged) {
                        var taskDto: TaskUpdateDTO = {
                            id: changedTask.id,
                            title: changedTask.title,
                            status: changedTask.status,
                            priority: changedTask.priority,
                            labelId: changedTask.labelId,
                        }
                        await updateTask(taskDto);
                    }
                    else if (changes.labelChanged) {
                        await updateLabel({ taskId: changedTask.id, newLabelId: changedTask.labelId! });
                    }
                    else if (changes.priorityChanged) {
                        await updatePriority({ taskId: changedTask.id, newPriority: changedTask.priority });
                    }
                })
            );

            discardAllChanges();
            toast.success('Değişiklikler kaydedildi');
        } catch (error) {
            console.error('Değişiklikleri kaydetme başarısız:', error);
            toast.error('Değişiklikleri kaydetme başarısız');
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleCancelChanges = () => {
        discardAllChanges();
        toast.info('Değişiklikler iptal edildi');
    };

    if (isLoading) return (
        <div className="flex flex-1 h-screen justify-center items-center">
            <ClipLoader size={64} color="rgb(var(--text))" />;
        </div>
    )

    return (
        <>
            <DndContext
                onDragStart={handleDragStart}
                onDragEnd={handleDragEnd}
                collisionDetection={pointerWithin}
                accessibility={{
                    announcements: {
                        onDragStart({ active }) {
                            return `Dragging ${active.id}`;
                        },
                        onDragOver({ active, over }) {
                            return `${active.id} over ${over?.id || 'nowhere'}`;
                        },
                        onDragEnd({ active, over }) {
                            return `${active.id} was dropped into ${over?.id || 'the void'}`;
                        },
                        onDragCancel() {
                            return `${activeId} drag cancelled`;
                        },
                    },
                }}
            >
                <div className="flex gap-6 overflow-x-auto overflow-y-hidden p-4 rounded-lg ">
                    {board.columns.map(column => (
                        <KanbanColumn key={column.id} column={column} />
                    ))}
                </div>

                <DragOverlay>
                    {activeDragTask ? <KanbanCard task={activeDragTask} isDragging /> : null}
                </DragOverlay>
            </DndContext>

            {
                hasChanges && (
                    <div className="fixed bottom-8 right-8 bg-yellow-50 border-2 border-yellow-200 rounded-lg p-4 shadow-lg z-50">
                        <p className="text-yellow-800 font-semibold mb-3">Kaydedilmemiş değişiklikler var</p>
                        <div className="flex justify-center gap-3">
                            <button onClick={handleConfirmChanges} disabled={isSubmitting} className="bg-green-500 hover:bg-green-600 disabled:bg-gray-400 text-white px-4 py-2 rounded-lg font-semibold cursor-pointer transition-colors">
                                {isSubmitting ? 'Kaydediliyor...' : 'Onayla'}
                            </button>
                            <button onClick={handleCancelChanges} disabled={isSubmitting} className="bg-red-500 hover:bg-red-600 disabled:bg-gray-400 text-white px-4 py-2 rounded-lg font-semibold cursor-pointer transition-colors">
                                İptal Et
                            </button>
                        </div>
                    </div>
                )}
        </>
    );
};

export default Kanban;