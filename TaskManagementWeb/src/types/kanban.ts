import type Label from "./label";
import type { Task, TaskPriority } from "./task";

export interface KanbanCard {
  id: string;
  task: Task;
}

export interface KanbanRow {
  priority: TaskPriority;
  cards: KanbanCard[];
}

export interface KanbanColumn {
  id: string;
  label: Label;
  rows: KanbanRow[];
}

export interface KanbanBoard {
  columns: KanbanColumn[];
}

export const PRIORITY_ORDER: Record<TaskPriority, number> = {
  "Urgent": 0,
  "High": 1,
  "Medium": 2,
  "Low": 3,
};