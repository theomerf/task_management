import { Link } from 'react-router-dom';
import logo from '../../assets/logo.png';
import logoDark from '../../assets/logo-darkmode.png';
import { useEffect, useRef, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../store/store';
import { Bell, ChevronUp } from 'lucide-react';
import { logout } from '../../pages/Account/accountSlice';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGear, faRightToBracket } from '@fortawesome/free-solid-svg-icons';
import { AnimatePresence, motion } from 'framer-motion';
import { useBreakpoint } from '../../hooks/useBreakpoint';

export default function Navbar() {
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
    const dropdownRef = useRef<HTMLDivElement>(null);
    const { user, preferences } = useAppSelector((state) => state.account);
    const dispatch = useAppDispatch();
    const { down } = useBreakpoint();
    const isMobile = down.sm;

    useEffect(() => {
        function handleClickOutside(event: MouseEvent) {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
                setIsDropdownOpen(false);
            }
        }

        document.addEventListener("mousedown", handleClickOutside);

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [dropdownRef]);

    return (
        <nav className="bg-[rgb(var(--div))] text-[rgb(var(--text))] border-b border-[rgb(var(--border))]/6 py-2 px-1 md:py-3 md:px-8 shadow-lg flex justify-between items-center">
            <Link to="/">
                <img src={preferences.darkMode ? logoDark : logo} className="object-cover w-36 md:w-48 transition-all duration-500 rounded-xl md:hover:scale-105 md:hover:bg-gray-100"></img>
            </Link>
            <div className="ml-auto flex flex-row items-center">
                <Link to="/notifications" title="Bildirimler" className="flex justify-center items-center w-6 h-6 md:w-10 md:h-10 rounded-full bg-linear-to-br from-orange-300 to-orange-400 text-white shadow-orange-300 shadow-md transition-all duration-500 md:hover:scale-105 hover:from-orange-400 hover:to-orange-500">
                    <Bell size={isMobile ? 16 : 24}/>
                </Link>
                <div ref={dropdownRef} className="relative">
                    <div onClick={() => setIsDropdownOpen(!isDropdownOpen)} className="flex flex-row ml-2 gap-x-1 md:gap-x-3 items-center cursor-pointer shadow-xl border border-[rgb(var(--border))]/6 bg-gray-100/20 bg-clip-padding backdrop-blur-lg backdrop-filter px-2 md:px-4 py-1 rounded-xl">
                        <div className="w-8 h-8 md:w-11 md:h-11 rounded-full shadow-md border-orange-400 border-2 transition-all duration-500 hover:scale-105">
                            <img src="https://i.hizliresim.com/ntfecdo.png" />
                        </div>
                        {!isMobile && <p className="md:text-lg">{user?.firstName} {user?.lastName}</p>}
                        <div className={`${isDropdownOpen ? 'transform rotate-180' : ''} transition-transform duration-300 text-gray-600`} >
                            <ChevronUp size={isMobile ? 16 : 24}/>
                        </div>
                    </div>
                    <AnimatePresence mode="wait">
                        {isDropdownOpen && <motion.div
                            initial={{ opacity: 0, scale: 0.95, y: -10 }}
                            animate={{ opacity: 1, scale: 1, y: 0 }}
                            exit={{ opacity: 0, scale: 0.95, y: -10 }}
                            transition={{ duration: 0.4, ease: "easeInOut" }}
                            className="flex flex-col items-center gap-y-1 md:gap-y-2 absolute top-full right-[-5%] p-2 md:px-4 md:py-6 z-10 bg-gray-100/20 bg-clip-padding backdrop-blur-lg backdrop-filter border border-[rgb(var(--border))]/6 shadow-lg rounded-lg">
                            <div className="w-12 h-12 md:w-16 md:h-16 rounded-full border-orange-400 border-2 transition-all duration-500 md:hover:scale-105">
                                <img src="https://i.hizliresim.com/ntfecdo.png" />
                            </div>
                            <div className="text-center bg-gray-200 backdrop-blur-xl rounded-lg p-1 md:p-2">
                                <p className="text-black">{user?.firstName} {user?.lastName}</p>
                                <p className="text-gray-600 text-xs md:text-[14px]">{user?.email}</p>
                            </div>
                            <div className="flex flex-col gap-y-1 md:gap-y-2 w-full">
                                <Link to="/settings" className="w-full text-center font-semibold bg-linear-to-br from-orange-400 via-orange-500 to-orange-600 text-white p-1 md:px-4 md:py-2 rounded-lg shadow-md shadow-orange-300 transition-all duration-500 hover:scale-[102%] hover:from-orange-500 hover:via-orange-600 hover:to-orange-700">
                                    <FontAwesomeIcon icon={faGear} className="md:mr-2" />
                                    Hesap Ayarları
                                </Link>
                                <button onClick={() => dispatch(logout())} className="w-full text-center font-semibold cursor-pointer bg-linear-to-br from-red-400 via-red-500 to-red-600 text-white p-1 md:px-4 md:py-2 rounded-lg shadow-md shadow-red-300 transition-all duration-500 hover:scale-[102%] hover:from-red-500 hover:via-red-600 hover:to-red-700">
                                    <FontAwesomeIcon icon={faRightToBracket} className="mr-2" />
                                    Çıkış Yap
                                </button>
                            </div>
                        </motion.div>
                        }
                    </AnimatePresence>
                </div>
            </div>
        </nav>
    );
}