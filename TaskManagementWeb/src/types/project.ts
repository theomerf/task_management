export interface Project {
    id: number;
    name: string;
    icon: string;
    color: string;
    status: string
    taskCount: number;
    completedTaskCount: number;
    createdAt: string;
    createdByEmail: string;
    memberCount: number;
}

export interface ProjectDetails {
    id: number;
    name: string;
    description: string | null;
    icon: string;
    color: string;
    createdAt: string;
    updatedAt: string;
    status: string
    visibility: string;
    taskCount: number;
    completedTaskCount: number;
}