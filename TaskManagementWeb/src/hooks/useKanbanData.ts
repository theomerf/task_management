import { useCallback, useMemo, useState } from 'react';
import type { Task, TaskPriority } from '../types/task';
import type Label from '../types/label';
import type { KanbanColumn, KanbanRow } from '../types/kanban';

interface ChangedTask {
    taskId: string;
    originalTask: Task;
    changedTask: Task;
    changes: {
        labelChanged: boolean;
        priorityChanged: boolean;
    };
}

interface UseKanbanDataProps {
    tasks: Task[] | undefined;
    labels: Label[] | undefined;
    isLoading: boolean;
}

export const useKanbanData = ({ tasks = [], labels = [], isLoading }: UseKanbanDataProps) => {
    const [changedTasks, setChangedTasks] = useState<Map<string, ChangedTask>>(new Map());

    const board = useMemo(() => {
        if (isLoading || !tasks.length || !labels.length) {
            return { columns: [] };
        }

        const columns: KanbanColumn[] = labels.map(label => {
            const priorities = ['Urgent', 'High', 'Medium', 'Low'] as const;
            const rows: KanbanRow[] = priorities.map(priority => {
                const labelTasks = tasks.filter(task => {
                    const changedTask = changedTasks.get(task.id);
                    const currentLabelId = changedTask ? changedTask.changedTask.labelId : task.labelId;
                    return currentLabelId === label.id;
                });

                return {
                    priority,
                    cards: labelTasks
                        .filter(task => {
                            const changedTask = changedTasks.get(task.id);
                            const currentPriority = changedTask ? changedTask.changedTask.priority : task.priority;
                            return currentPriority === priority;
                        })
                        .map(task => {
                            const changedTask = changedTasks.get(task.id);
                            const displayTask = changedTask ? changedTask.changedTask : task;

                            return {
                                id: task.id,
                                task: displayTask,
                            };
                        }),
                };
            });

            return {
                id: label.id,
                label,
                rows,
            };
        });

        return { columns };
    }, [tasks, labels, isLoading, changedTasks]);

    const updateTaskPosition = useCallback((taskId: string, newPriority: string, newLabelId: string) => {
        const originalTask = tasks?.find(t => t.id === taskId);
        if (!originalTask) return;

        setChangedTasks(prev => {
            const updated = new Map(prev);

            const labelChanged = originalTask.labelId !== newLabelId;
            const priorityChanged = originalTask.priority !== newPriority;

            if (!labelChanged && !priorityChanged) {
                updated.delete(taskId);
                return updated;
            }

            const changedTask = {
                ...originalTask,
                priority: newPriority as TaskPriority,
                labelId: newLabelId,
            };

            updated.set(taskId, {
                taskId,
                originalTask,
                changedTask,
                changes: {
                    labelChanged,
                    priorityChanged
                },
            });
            
            return updated;
        });
    }, [tasks]);

    const discardChanges = useCallback((taskId: string) => {
        setChangedTasks(prev => {
            const updated = new Map(prev);
            updated.delete(taskId);
            return updated;
        });
    }, []);

    const discardAllChanges = useCallback(() => {
        setChangedTasks(new Map());
    }, []);

    const getChangedTasks = useCallback(() => {
        return Array.from(changedTasks.values());
    }, [changedTasks]);

    const hasChanges = changedTasks.size > 0;

    return { board, updateTaskPosition, discardChanges, discardAllChanges, getChangedTasks, hasChanges };
};