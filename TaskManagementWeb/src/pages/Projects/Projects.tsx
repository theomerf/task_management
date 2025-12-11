import { useQuery } from "@tanstack/react-query";
import requests from "../../services/api";
import type { Project } from "../../types/project";
import { FolderPlus } from "lucide-react";
import { useBreakpoint } from "../../hooks/useBreakpoint";
import { ClipLoader } from "react-spinners";
import { useEffect } from "react";
import { toast } from "react-toastify";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCalendar, faPen } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";

export default function Projects() {
    const { data: projects, isLoading, error } = useQuery({
        queryKey: ['projects'],
        queryFn: async ({ signal }): Promise<Project[]> => {
            var projects = await requests.project.me(signal) as Project[];
            return projects.map((project) => {
                project.status = project.status == 'Active' ? "Aktif" :
                    (project.status == 'Completed' ? "Tamamlandı" :
                        (project.status == 'OnHold' ? "Beklemede" : "Arşivlendi"));
                return project;
            })
        },
        refetchOnWindowFocus: false,
        staleTime: 1000 * 60 * 5,
        gcTime: 1000 * 60 * 10,
    });
    const { down } = useBreakpoint();
    const isMobile = down.sm;

    useEffect(() => {
        if (error) {
            toast.error('Projeler yüklenirken bir hata oluştu. Lütfen sayfayı yenileyin.');
            console.log(error);
        }
    }, [error]);

    return (
        <div className="flex flex-col w-full">
            <div className="ml-auto">
                <button aria-label="Yeni Proje" className="flex flex-row gap-x-2 p-2 bg-linear-to-br from-lime-400 via-lime-500 to-lime-600 shadow-lime-300 shadow-md rounded-lg text-white font-semibold transition-all duration-500 hover:from-lime-500 hover:via-lime-600 hover:to-lime-700 hover:scale-105 cursor-pointer">
                    <FolderPlus />
                    {!isMobile && 'Yeni Proje'}
                </button>
            </div>
            <div className="w-full">
                {isLoading && (
                    <div className="flex flex-1 justify-center items-center">
                        <ClipLoader size={48} color="rgb(var(--text))" />;
                    </div>
                )}

                {!isLoading && projects?.length === 0 && (
                    <div className="bg-[rgb(var(--div))]/60 border border-[rgb(var(--border))]/6 p-6 justify-center items-center text-lg text-center rounded-lg mt-6">
                        <p>
                            Henüz bir projeniz yok. Başlamak için "Yeni Proje" butonuna tıklayın!
                        </p>
                    </div>
                )}

                {!isLoading && projects?.length! > 0 && (
                    <div className="grid grid-cols-1 lg:grid-cols-3 w-full gap-x-6">
                        {projects?.map((project) => (
                            <Link to={`/projects/${project.id}`} key={project.id} style={{ backgroundColor: project.color }} className="flex flex-col gap-y-3 text-white border-[rgb(var(--border))]/6 p-4 rounded-lg shadow-lg transition-all duration-500 hover:scale-[102%]">
                                <div className="flex flex-row items-center">
                                    <p className="lg:text-4xl bg-white rounded-lg p-1">{project.icon}</p>
                                    <h2 className="w-full text-center text-2xl font-semibold">{project.name}</h2>
                                </div>
                                <div className={`${project.status == 'Aktif' ? 'bg-green-500' : project.status == 'Tamamlandı' ? 'bg-black' : project.status == 'Beklemede' ? 'bg-yellow-500' : 'bg-amber-800'} w-fit self-center px-4 py shadow-md text-center rounded-lg`}>
                                    <p className="text-lg">{project.status}</p>
                                </div>
                                <div className="w-fit p-2 mt-6 rounded-lg shadow-md self-center bg-white text-black font-semibold">
                                    <div className="flex flex-row items-center">
                                        <FontAwesomeIcon icon={faPen} className="text-blue-500 text-lg mr-1" />
                                        <p>{project.createdByEmail}</p>
                                    </div>
                                    <div className="flex flex-row justify-center items-center">
                                        <FontAwesomeIcon icon={faCalendar} className="text-blue-500 text-lg mr-1" />
                                        <p>{new Date(project.createdAt).toLocaleDateString()}</p>
                                    </div>
                                </div>
                                <div className="flex flex-row justify-between mt-6">
                                    <div className="flex flex-col items-center">
                                        <p className="font-semibold text-lg">{project.taskCount}</p>
                                        <p className="font-semibold">Toplam Görev</p>
                                    </div>
                                    <div className="flex flex-col items-center">
                                        <p className="font-semibold text-lg">{project.completedTaskCount}</p>
                                        <p className="font-semibold">Tamamlanan Görev</p>
                                    </div>
                                    <div className="flex flex-col items-center">
                                        <p className="font-semibold text-lg">{project.memberCount}</p>
                                        <p className="font-semibold">Toplam Üye</p>
                                    </div>
                                </div>
                            </Link>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}