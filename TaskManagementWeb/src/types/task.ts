import type Label from "./label";

export interface Task {
    id: string;
    createdByEmail: string;
    assignedToEmail: string;
    title: string;
    labelId?: string | null;
    status: TaskStatus;
    priority: TaskPriority;
    progressPercentage: number;
    commentCount: number;
    attachmentCount: number;
}

export interface TaskDetails {
    id: string;
    createdByEmail: string;
    createdByFirstName: string;
    createdByLastName: string;
    assignedToEmail: string;
    assignedToFirstName: string;
    assignedToLastName: string;
    labelName : string;
    labelColor : string;
    title: string;
    description: string | null;
    status: TaskStatus;
    priority: TaskPriority;
    startDate: string | null;
    dueDate: string | null;
    completedAt: string | null;
    createdAt: string;
    updatedAt: string;
    estimatedHours: number | null;
    totalHoursSpent: number;
    progressPercentage: number;
}

export interface TaskUpdateDTO {
    id: string;
    assignedToId?: string | null;
    labelId?: string | null;
    description?: string | null;
    title: string;
    status: TaskStatus;
    priority: TaskPriority;
    startDate?: string | null;
    dueDate?: string | null;
    completedAt?: string | null;
}

export const TaskStatus = {
    ToDo: "ToDo",
    InProgress: "InProgress",
    InReview: "InReview",
    Done: "Done",
    Blocked: "Blocked",
} as const;

export type TaskStatus = typeof TaskStatus[keyof typeof TaskStatus];

export const TaskPriority = {
    Low: "Low",
    Medium: "Medium",
    High: "High",
    Urgent: "Urgent",
} as const;

export type TaskPriority = typeof TaskPriority[keyof typeof TaskPriority];

export interface KanbanColumn {
  id: string;
  label: Label;
  tasks: Task[];
}

const priorityOrder: Record<TaskPriority, number> = {
    "Low": 0,
    "Medium": 1,
    "High": 2,
    "Urgent": 3
};

export const getPriorityOrder = (priority:  TaskPriority): number => {
    return priorityOrder[priority];
};

export const getPriorityLabel = (priority: TaskPriority): string => {
    const labels:  Record<TaskPriority, string> = {
        "Low":  "Düşük",
        "Medium":  "Orta",
        "High": "Yüksek",
        "Urgent": "Acil"
    };
    return labels[priority];
};

export const getStatusLabel = (status: TaskStatus): string => {
    const labels: Record<TaskStatus, string> = {
        "ToDo": "Yapılacak",
        "InProgress": "Devam Ediyor",
        "InReview": "İnceleme",
        "Done": "Tamamlandı",
        "Blocked": "Bloklandı"
    };
    return labels[status];
};