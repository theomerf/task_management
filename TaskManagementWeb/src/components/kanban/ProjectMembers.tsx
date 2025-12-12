import { faCheck, faEdit, faHammer, faMinus, faUserPlus, faX, faXmarkCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type ProjectMember from "../../types/projectMember";
import requests from "../../services/api";
import { ClipLoader } from "react-spinners";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { getAllProjectMemberRoles, getProjectMeberRole, ProjectMemberRole } from "../../types/projectMember";
import { useForm } from "react-hook-form";

type ProjectMembersProps = {
    projectId: string;
    setIsMembersOpen: (isOpen: boolean) => void;
}

export default function ProjectMembers({ projectId, setIsMembersOpen }: ProjectMembersProps) {
    const queryClient = useQueryClient();
    const { register, handleSubmit, formState: { errors } } = useForm<{
        email: string;
        role: string;
    }>();
    const [newRole, setNewRole] = useState<string>();
    const [addMember, setAddMember] = useState(false);
    const { data: members, isLoading, error } = useQuery({
        queryKey: ['project', projectId, 'members'],
        queryFn: async ({ signal }): Promise<ProjectMember[]> => {
            return await requests.project.getMembers(projectId, signal);
        },
        refetchOnWindowFocus: false,
        staleTime: 1000 * 60 * 5,
        gcTime: 1000 * 60 * 10,
    });

    const handleMemberAdd = useMutation({
        mutationFn: async (data: { email: string, role: string }) => {
            await requests.project.addMember(projectId, data);
        },
        onSuccess: () => {
            setAddMember(false);
            queryClient.invalidateQueries({ queryKey: ['project', projectId, 'members'] });
            toast.success("Üye başarıyla projeye eklendi.");
        },
        onError: (error) => {
            setAddMember(false);
            console.error("Üye projeye eklenirken bir hata oluştu:", error);
            toast.error("Üye projeye eklenirken bir hata oluştu.");
        }
    });

    const handleRoleUpdate = useMutation({
        mutationFn: async (data: { id: string, role: string }) => {
            if (newRole === undefined)
                throw new Error("Yeni rol belirtilmemiş.");
            else if (newRole === members?.find(m => m.id === data.id)?.role)
                throw new Error("Yeni rol mevcut rolden farklı olmalıdır.");

            await requests.project.updateMember(projectId, data);
        },
        onSuccess: () => {
            setNewRole(undefined);
            queryClient.invalidateQueries({ queryKey: ['project', projectId, 'members'] });
            toast.success("Üye rolü başarıyla güncellendi.");
        },
        onError: (error) => {
            if (error instanceof Error && error.message === "Yeni rol belirtilmemiş.")
                toast.error("Lütfen bir rol seçin.");
            else if (error instanceof Error && error.message === "Yeni rol mevcut rolden farklı olmalıdır.")
                toast.error("Yeni rol, mevcut rolden farklı olmalıdır.");
            else {
                setNewRole(undefined);
                console.error("Üye rolü güncellenirken bir hata oluştu:", error);
                toast.error("Üye rolü güncellenirken bir hata oluştu.");
            }
        }
    });

    const handleMemberRemove = useMutation({
        mutationFn: async (id: string) => {
            await requests.project.removeMember(projectId, id);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['project', projectId, 'members'] });
            toast.success("Üye başarıyla projeden çıkarıldı.");
        },
        onError: (error) => {
            console.error("Üye projeden çıkarılırken bir hata oluştu:", error);
            toast.error("Üye projeden çıkarılırken bir hata oluştu.");
        }
    });

    useEffect(() => {
        if (error) {
            console.error("Proje üyeleri alınırken bir hata oluştu:", error);
            toast.error("Proje üyeleri alınırken bir hata oluştu.");
        }
    }, [error])

    return (
        <div className="fixed flex inset-0 bg-black/50 backdrop-blur-sm justify-center items-center z-50">
            <div className="flex flex-col relative bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 text-[rgb(var(--text))] p-6 rounded-lg overflow-hidden shadow-md">
                <div>
                    <h2 className="col-span-2 text-2xl font-semibold text-center">{addMember ? 'Üye Ekle' : 'Proje Üyeleri'}</h2>
                    <button onClick={() => setIsMembersOpen(false)} title="Kapat" className="absolute top-0 right-0 bg-red-500 text-white rounded-bl-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105 hover:bg-red-600">
                        <FontAwesomeIcon icon={faXmarkCircle} />
                    </button>
                </div>
                {!addMember ? (
                    <>
                        <button onClick={() => setAddMember(true)} title="Üye Ekle" className="self-end my-[-15px] w-fit mr-5 bg-green-500 border border-[rgb(var(--border))]/6 text-white shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                            <FontAwesomeIcon icon={faUserPlus} />
                        </button>
                        <div className="md:max-h-80 overflow-y-auto mt-5">
                            {isLoading && (
                                <div className="flex flex-1 justify-center items-center">
                                    <ClipLoader size={48} color="rgb(var(--text))" />;
                                </div>
                            )}

                            {!isLoading && members?.length === 0 && (
                                <p>Bu projede henüz üye yok.</p>
                            )}

                            {!isLoading && members && members.length > 0 && members.map((member) => (
                                <div className="grid grid-cols-5 items-center w-full p-2 border-2 border-gray-200">
                                    <div className="col-span-2 flex flex-row gap-x-2">
                                        <img src={member.accountAvatarUrl} alt={`${member.accountFirstName}'nin profil fotoğrafı`} className="w-12 h-12 transition-all duration-500 hover:scale-105" />
                                        <div className="flex flex-col">
                                            <p className="font-bold">{member.accountFirstName} {member.accountLastName}</p>
                                            <p className="text-sm text-gray-500">{member.accountEmail}</p>
                                        </div>
                                    </div>
                                    <div className="col-span-2 flex flex-row justify-center items-center gap-x-2">
                                        {newRole ? (
                                            <select onChange={(e) => setNewRole(e.target.value)} value={newRole} className="outline-none border-2 rounded-lg border-gray-200 text-[rgb(var(--text))] p-2">
                                                {getAllProjectMemberRoles().map((role) => (
                                                    <option key={role} value={role} className="bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 text-[rgb(var(--text))]">{getProjectMeberRole(role)}</option>
                                                ))}
                                            </select>
                                        ) : (
                                            <p className="text-sm">{getProjectMeberRole(member.role)}</p>
                                        )}
                                        <p className="text-sm">{new Date(member.joinedAt).toLocaleDateString()}</p>
                                    </div>
                                    <div className="col-span-1 flex flex-row gap-x-2 justify-center">
                                        {newRole ? (
                                            <>
                                                <button onClick={async () => await handleRoleUpdate.mutateAsync({ id: member.id, role: newRole })} title="Onayla" className="bg-green-500 border border-[rgb(var(--border))]/6 text-white shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                                    <FontAwesomeIcon icon={faCheck} />
                                                </button>
                                                <button onClick={() => setNewRole(undefined)} title="İptal Et" className="bg-red-500 border border-[rgb(var(--border))]/6 text-white shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                                    <FontAwesomeIcon icon={faX} />
                                                </button>
                                            </>
                                        ) : (
                                            <>
                                                <button onClick={() => setNewRole(member.role)} title="Rolü Güncelle" className="bg-yellow-500 border border-[rgb(var(--border))]/6 text-white shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                                    <FontAwesomeIcon icon={faHammer} />
                                                </button>
                                                <button onClick={async () => await handleMemberRemove.mutateAsync(member.id)} title="Projeden Çıkar" className="bg-red-500 border border-[rgb(var(--border))]/6 text-white shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                                    <FontAwesomeIcon icon={faMinus} />
                                                </button>
                                            </>
                                        )}
                                    </div>

                                </div>
                            ))}

                        </div>
                    </>
                ) : (
                    <div>
                        <form method="POST" onSubmit={handleSubmit((data) => handleMemberAdd.mutate(data))}>
                            <div className="flex flex-col gap-y-4 px-6 py-2">
                                <div className="flex flex-col gap-y-2">
                                    <label htmlFor="email" className="font-semibold">E-Posta</label>
                                    <input type="email" id="email"{...register("email", {
                                        required: "E-Posta zorunludur.",
                                        pattern: {
                                            value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
                                            message: "Geçersiz e-posta adresi."
                                        }
                                    })} placeholder="Üye E-Postasını Giriniz" className="border-gray-400 border rounded-lg p-2 outline-none w-full transition-all duration-500 focus:scale-[103%] focus:border-black" />
                                    {errors.email && <p className="text-red-500 text-sm">{errors.email.message?.toString()}</p>}
                                </div>
                                <div className="flex flex-col gap-y-2">
                                    <label htmlFor="email" className="font-semibold">Rol</label>
                                    <select {...register("role", {
                                        required: "Rol alanı zorunludur."
                                    })} className="outline-none border-2 rounded-lg border-gray-200 text-[rgb(var(--text))] p-2">
                                        <option value="" disabled selected>Rol Seçiniz</option>
                                        {getAllProjectMemberRoles().map((role) => (
                                            <option key={role} value={role} className="bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 text-[rgb(var(--text))]">{getProjectMeberRole(role)}</option>
                                        ))}
                                    </select>
                                    {errors.email && <p className="text-red-500 text-sm">{errors.email.message?.toString()}</p>}
                                </div>
                                <div className="flex justify-center gap-x-2 mt-4">
                                    <button type="submit" title="Onayla" className="bg-green-500 border border-[rgb(var(--border))]/6 text-white shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                        <FontAwesomeIcon icon={faCheck} />
                                    </button>
                                    <button onClick={() => setAddMember(false)} title="İptal Et" className="bg-red-500 border border-[rgb(var(--border))]/6 text-white shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                        <FontAwesomeIcon icon={faX} />
                                    </button>
                                </div>
                            </div>
                        </form>
                    </div>
                )}
            </div>
        </div>
    );
}