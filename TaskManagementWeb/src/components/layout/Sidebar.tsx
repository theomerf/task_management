import { ClipboardList, Folders, House, Moon, Settings, Sun } from "lucide-react";
import { NavLink } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../store/store";
import { setPreferences } from "../../pages/Account/accountSlice";
import { useBreakpoint } from "../../hooks/useBreakpoint";

export default function Sidebar() {
    const { preferences } = useAppSelector((state) => state.account);
    const dispatch = useAppDispatch();
    const { down } = useBreakpoint();
    const isMobile = down.sm;

    const handleUserPreferenceChange = () => {
        dispatch(setPreferences({
            ...preferences,
            darkMode: !preferences.darkMode
        }))
    };

    return (
        <nav className="flex flex-col justify-center items-center gap-y-2 h-full bg-[rgb(var(--div))] border-[rgb(var(--border))]/6 border border-t-0 text-white py-4 md:px-2 px-1 shadow-lg rounded-br-lg">
            <NavLink to="/" title="Ana Sayfa" aria-label="Ana Sayfa" className={({ isActive }) => `${isActive ? 'border-2 border-white scale-110' : ''} flex justify-center items-center w-10 h-10 md:w-12 md:h-12 rounded-lg bg-linear-to-br from-red-400 via-red-500 to-red-600 shadow-md shadow-red-300 active:scale-105 transition-all duration-500 md:hover:scale-105 hover:from-red-500 hover:via-red-600 hover:to-red-700`}>
                <House size={isMobile ? 20 : 24} className="hover:rotate-12 transition-transform duration-500" />
            </NavLink>
            <NavLink to="/projects" title="Projeler" aria-label="Projeler" className={({ isActive }) => `${isActive ? 'border-2 border-white scale-110' : ''} flex justify-center items-center w-10 h-10 md:w-12 md:h-12 rounded-lg bg-linear-to-br from-orange-400 via-orange-500 to-orange-600 shadow-md shadow-orange-300 active:scale-105 transition-all duration-500 md:hover:scale-105 hover:from-orange-500 hover:via-orange-600 hover:to-orange-700`}>
                <Folders size={isMobile ? 20 : 24} className="hover:rotate-12 transition-transform duration-500" />
            </NavLink>
            <NavLink to="/tasks" title="Görevlerim" aria-label="Görevler" className={({ isActive }) => `${isActive ? 'border-2 border-white scale-110' : ''} flex justify-center items-center w-10 h-10 md:w-12 md:h-12 rounded-lg bg-linear-to-br from-lime-400 via-lime-500 to-lime-600 shadow-md shadow-lime-300 active:scale-105 transition-all duration-500 md:hover:scale-105 hover:from-lime-500 hover:via-lime-600 hover:to-lime-700`}>
                <ClipboardList size={isMobile ? 20 : 24} className="hover:rotate-12 transition-transform duration-500" />
            </NavLink>
            <NavLink to="/settings" title="Ayarlar" aria-label="Ayarlar" className={({ isActive }) => `${isActive ? 'border-2 border-white scale-110' : ''} flex justify-center items-center w-10 h-10 md:w-12 md:h-12 rounded-lg bg-linear-to-br from-blue-400 via-blue-500 to-blue-600 shadow-md shadow-blue-300 active:scale-105 transition-all duration-500 md:hover:scale-105 hover:from-blue-500 hover:via-blue-600 hover:to-blue-700`}>
                <Settings size={isMobile ? 20 : 24} className="hover:rotate-12 transition-transform duration-500" />
            </NavLink>
            <div className="border-t-2 border-[rgb(var(--text))] w-full"></div>

            <button onClick={handleUserPreferenceChange} title={preferences.darkMode ? 'Gündüz Moduna Geç' : 'Gece Moduna Geç'} className="flex mt-auto cursor-pointer justify-center items-center w-8 h-8 md:w-10 md:h-10 rounded-full bg-linear-to-br from-orange-300 to-orange-400 text-white transition-all duration-500 md:hover:scale-105 hover:from-orange-400 hover:to-orange-500">
                {preferences.darkMode ? (
                    <Moon />
                ) : (
                    <Sun />
                )}
            </button>
        </nav>
    );
}