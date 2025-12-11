import { useDroppable } from '@dnd-kit/core';
import type { KanbanColumn as KanbanColumnType } from '../../types/kanban';
import KanbanRow from './KanbanRow';
import StickyNote from './StickyNote';

interface KanbanColumnProps {
  column: KanbanColumnType;
}

export default function KanbanColumn({ column }: KanbanColumnProps) {
  const { setNodeRef } = useDroppable({
    id: column.id,
  });

  return (
    <div ref={setNodeRef} className="flex flex-col gap-4 shrink-0 w-80">
      <div className="flex w-full justify-center">
        <StickyNote color={column.label.color} rotation={0}>
          <p className="text-white font-extrabold md:px-3">{column.label.name}</p>
        </StickyNote>
      </div>
      <div className="flex flex-col gap-6">
        {column.rows.map(row => (
          <KanbanRow key={row.priority} row={row} labelId={column.id} color={column.label.color}/>
        ))}
      </div>
    </div>
  );
};