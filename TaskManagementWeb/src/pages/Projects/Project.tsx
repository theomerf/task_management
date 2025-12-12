import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { ProjectDetails } from "../../types/project";
import requests from "../../services/api";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import Kanban from "../../components/kanban/Kanban";
import { ClipLoader } from "react-spinners";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFileLines, faGears, faUsers } from "@fortawesome/free-solid-svg-icons";
import cork from "../../assets/cork.jpg";
import StickyNote from "../../components/kanban/StickyNote";
import { toast } from "react-toastify";
import type { TaskUpdateDTO } from "../../types/task";
import ProjectMembers from "../../components/kanban/ProjectMembers";
import { AnimatePresence } from "framer-motion";
import { motion } from "framer-motion";

export default function Project() {
    const projectId = useParams().projectId!;
    const queryClient = useQueryClient();
    const [isMembersOpen, setIsMembersOpen] = useState(false);
    const [isSettingsOpen, setIsSettingsOpen] = useState(false);
    const { data: project, isLoading: projectLoading, error: projectError } = useQuery({
        queryKey: ['project', projectId],
        queryFn: async ({ signal }): Promise<ProjectDetails> => {
            return await requests.project.getOne(projectId!, signal);
        },
        refetchOnWindowFocus: false,
        staleTime: 1000 * 60 * 5,
        gcTime: 1000 * 60 * 10,
    })

    const { data: tasks, isLoading: tasksLoading, error: tasksError } = useQuery({
        queryKey: ['project', projectId, 'tasks'],
        queryFn: async ({ signal }) => {
            return (await requests.task.getAll(projectId!, {}, signal));
        },
        refetchOnWindowFocus: false,
        staleTime: 1000 * 60 * 2,
        gcTime: 1000 * 60 * 5,
        enabled: project !== undefined,
    });

    const handleTaskUpdate = useMutation({
        mutationFn: async (data: TaskUpdateDTO) => {
            return await requests.task.update(projectId, data);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['project', projectId, 'tasks'] });
        },
        onError: (error) => {
            console.error('Görev güncelleme hatası:', error);
            toast.error('Görev güncellenirken bir hata oluştu.');
        }
    });

    const handleTaskPriorityUpdate = useMutation({
        mutationFn: async (data: { id: string; priority: string }) => {
            return await requests.task.updatePriority(projectId, {
                id: data.id,
                priority: data.priority,
            });
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['project', projectId, 'tasks'] });
        },
        onError: (error) => {
            console.error('Görev önceliği güncelleme hatası:', error);
            toast.error('Görev önceliği güncellenirken bir hata oluştu.');
        }
    });

    const handleTaskLabelUpdate = useMutation({
        mutationFn: async (data: { id: string; labelId: string }) => {
            return await requests.task.updateLabel(projectId, {
                id: data.id,
                labelId: data.labelId,
            });
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['project', projectId, 'tasks'] });
        },
        onError: (error) => {
            console.error('Görev etiket güncelleme hatası:', error);
            toast.error('Görev etiketi güncellenirken bir hata oluştu.');
        }
    });

    const { data: labels, isLoading: labelsLoading, error: labelsError } = useQuery({
        queryKey: ['project', projectId, 'labels'],
        queryFn: async ({ signal }) => {
            return (await requests.project.getLabels(projectId!, signal));
        },
        refetchOnWindowFocus: false,
        staleTime: 1000 * 60 * 10,
        gcTime: 1000 * 60 * 15,
        enabled: project !== undefined,
    });

    useEffect(() => {
        if (projectError) {
            console.error('Proje yükleme hatası:', projectError);
        }
        if (tasksError) {
            console.error('Görevler yükleme hatası:', tasksError);
        }
        if (labelsError) {
            console.error('Etiketler yükleme hatası:', labelsError);
        }
    }, [projectError, tasksError, labelsError]);

    return (
        <>
            <div className="relative w-full min-h-[600px] rounded-lg shadow-2xl overflow-hidden">
                <div className="absolute inset-0 bg-[#caa478] rounded-lg" />
                <div className="absolute inset-1 rounded-lg shadow-inner" />
                <div className="absolute inset-4 rounded bg-cover bg-center" style={{ backgroundImage: `url(${cork})` }}>
                    <div className="absolute inset-0 bg-linear-to-b from-black/5 via-transparent to-black/10 rounded" />
                    <div className="absolute inset-0 rounded shadow-[inset_0_0_15px_rgba(0,0,0,0.3)]" />
                </div>
                <div className="relative flex flex-col w-full h-full p-10">
                    <div>
                        {projectLoading && (
                            <div className="flex flex-1 justify-center items-center">
                                <ClipLoader size={48} color="rgb(var(--text))" />;
                            </div>
                        )}
                        {!projectLoading && project && (
                            <div className="grid grid-cols-4">
                                <div className="col-span-1">
                                    <StickyNote color={project.color} rotation={0}>
                                        <div className="flex flex-row justify-center items-center px-3">
                                            <p className="text-3xl text-white">{project.icon}</p>
                                            <p className="text-xl font-extrabold text-white">{project.name}</p>
                                        </div>
                                    </StickyNote>
                                </div>
                                <div className="flex col-span-2 justify-center">
                                    <StickyNote color={project.color} rotation={0}>
                                        <p className="text-2xl font-extrabold text-white text-center px-20">Kanban Board</p>
                                    </StickyNote>
                                </div>
                                <div className="col-span-1 flex justify-end">
                                    <StickyNote color={project.color} rotation={0}>
                                        <div className="flex flex-row gap-x-3 px-2">
                                            <button onClick={() => setIsMembersOpen(true)} title="Üyeler" className="bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 text-[rgb(var(--text))] shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                                <FontAwesomeIcon icon={faUsers} />
                                            </button>
                                            <button title="Aktivite Logları" className="bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 text-[rgb(var(--text))] shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                                <FontAwesomeIcon icon={faFileLines} />
                                            </button>
                                            <button title="Ayarlar" className="bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 text-[rgb(var(--text))] shadow-lg rounded-lg px-2 py-1 cursor-pointer transition-all duration-500 hover:scale-105">
                                                <FontAwesomeIcon icon={faGears} />
                                            </button>
                                        </div>
                                    </StickyNote>
                                </div>
                            </div>
                        )}
                    </div>
                    <div className="flex-1 overflow-auto p-6">
                        <Kanban tasks={tasks} labels={labels} isLoading={projectLoading || tasksLoading || labelsLoading} updateTask={handleTaskUpdate.mutateAsync} updatePriority={handleTaskPriorityUpdate.mutateAsync} updateLabel={handleTaskLabelUpdate.mutateAsync} />
                    </div>
                </div>
            </div>
            <AnimatePresence mode="wait">
                {isMembersOpen && <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }} transition={{ duration: 0.3, ease: 'easeInOut' }}>
                    <ProjectMembers projectId={projectId} setIsMembersOpen={setIsMembersOpen} />
                </motion.div>}
            </AnimatePresence>

        </>
    );
}