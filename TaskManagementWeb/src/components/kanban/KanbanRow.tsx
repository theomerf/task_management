import { useDroppable } from '@dnd-kit/core';
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable';
import type { KanbanRow as KanbanRowType } from '../../types/kanban';
import KanbanCard from './KanbanCard';
import { getPriorityLabel } from '../../types/task';
import StickyNote from './StickyNote';

interface KanbanRowProps {
  row: KanbanRowType;
  labelId: string;
  color: string;
}

export default function KanbanRow({ row, labelId, color }: KanbanRowProps) {
  const { setNodeRef } = useDroppable({
    id: `${row.priority}/${labelId}`,
  });

  return (
    <div className="flex flex-col gap-2">
      <StickyNote color={color} rotation={0}>
        <p className="text-white font-semibold">{getPriorityLabel(row.priority)}</p>
      </StickyNote>

      <div ref={setNodeRef} className="flex flex-col gap-3 p-3 rounded-lg bg-[rgb(var(--div))]/20 min-h-36">
        <SortableContext items={row.cards.map(c => c.id)} strategy={verticalListSortingStrategy}>
          {row.cards.length > 0 ? (
            row.cards.map(card => (
              <KanbanCard key={card.id} task={card.task} />
            ))
          ) : (
            <div className="text-sm text[rgb(var(--text))] text-center py-8">
              Görev yok
            </div>
          )}
        </SortableContext>
      </div>
    </div>
  );
};