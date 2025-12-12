export default interface ProjectMember {
    id: string;
    accountId: string;
    role: ProjectMemberRole;
    joinedAt: string;
    leftAt?: string;
    isActive: boolean;
    accountFirstName: string;
    accountLastName: string;
    accountEmail: string;
    accountAvatarUrl: string;
};

export const ProjectMemberRole = {
    Owner: "Owner",
    Manager: "Manager",
    Member: "Member"
} as const;

export type ProjectMemberRole = typeof ProjectMemberRole[keyof typeof ProjectMemberRole];

export const getProjectMeberRole = (role: ProjectMemberRole): string => {
    const roles: Record<ProjectMemberRole, string> = {
        "Owner": "Proje Sahibi",
        "Manager": "Proje Yöneticisi",
        "Member": "Proje Üyesi"
    };
    return roles[role];
};

export const getAllProjectMemberRoles = (): ProjectMemberRole[] => {
    return Object.values(ProjectMemberRole);
}