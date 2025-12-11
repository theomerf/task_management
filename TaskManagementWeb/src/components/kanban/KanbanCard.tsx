import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import type { Task } from '../../types/task';
import { getPriorityLabel, getStatusLabel } from '../../types/task';
import StickyNote from './StickyNote';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faComment, faLink } from '@fortawesome/free-solid-svg-icons';

interface KanbanCardProps {
    task: Task;
    isDragging?: boolean;
}

const KanbanCard = ({ task, isDragging }: KanbanCardProps) => {
    const {
        attributes,
        listeners,
        setNodeRef,
        transform,
        transition,
        isDragging: isSortableDragging,
    } = useSortable({ id: task.id });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
        opacity: isSortableDragging ? 0.5 : 1,
    };

    return (
        <div ref={setNodeRef} style={style} {...attributes} {...listeners}>
            <StickyNote color={'#FFF'} rotation={-1} canGrab>
                <div className="flex flex-row justify-between">
                    <h4 className="font-semibold text-sm text-black truncate">
                        {task.title}
                    </h4>
                    <div title="İlerleme" className="relative ml-auto w-10 h-5 border-2 border-gray-400 rounded-sm">
                        <div className={`bg-green-500 h-full w-[10%]`}></div>
                        <div className="absolute -top-0.5 left-2.5">
                            {`${Math.min((task.progressPercentage || 0), 100)}%`}
                        </div>
                    </div>
                </div>


                <div className="flex gap-2 mt-2 flex-wrap">
                    <span className="text-xs px-2 py-1 rounded bg-gray-100 text-gray-700">
                        {getPriorityLabel(task.priority)}
                    </span>
                    <span className="text-xs px-2 py-1 rounded bg-blue-100 text-blue-700">
                        {getStatusLabel(task.status)}
                    </span>
                </div>

                <div className="flex justify-between items-center mt-3 text-xs text-gray-500 dark:text-gray-400">
                    <div className="flex gap-2">
                        {task.commentCount > 0 && <span><FontAwesomeIcon icon={faComment} className="text-gray-300" /> {task.commentCount}</span>}
                        {task.attachmentCount > 0 && <span><FontAwesomeIcon icon={faLink} className="text-gray-300" /> {task.attachmentCount}</span>}
                    </div>
                    <div title={`Oluşturan Kişi: ${task.createdByEmail}\n Atanan Kişi: ${task.assignedToEmail}`} className="w-6 h-6 rounded-full bg-linear-to-br from-blue-400 to-purple-500 flex items-center justify-center text-white text-xs font-bold">
                        {task.assignedToEmail?.[0]?.toUpperCase()}
                    </div>
                </div>
            </StickyNote>
        </div>
    );
};

export default KanbanCard;