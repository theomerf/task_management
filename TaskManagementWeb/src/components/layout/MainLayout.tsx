import { Outlet, useLocation } from "react-router-dom";
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";
import { AnimatePresence, motion } from "framer-motion";

export default function MainLayout() {
    const location = useLocation();

    return (
        <div className="flex flex-col h-screen overflow-hidden">
            <header className="w-full">
                <Navbar />
            </header>
            <div className="flex flex-row h-full">
                <aside>
                    <Sidebar />
                </aside>
                <main className="flex flex-1 overflow-y-auto p-6">
                    <AnimatePresence key={location.pathname}  mode="wait">
                        <motion.div
                            key={location.pathname}
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.5, ease: "easeInOut" }}
                            className="flex flex-1 "
                        >
                            <Outlet />
                        </motion.div>
                    </AnimatePresence>
                </main>
            </div>
        </div>
    );
}