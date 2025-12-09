import { Route, Routes, unstable_HistoryRouter as HistoryRouter } from 'react-router-dom'
import './App.css'
import { history } from './utils/history'
import Login from './pages/Account/Login'
import ProtectedRoute from './utils/ProtectedRoute'
import { useEffect } from 'react'
import { useAppSelector, type AppDispatch } from './store/store'
import { useDispatch } from 'react-redux'
import { checkAuth, setPreferences, setUser } from './pages/Account/accountSlice'
import Register from './pages/Account/Register'
import AccountLayout from './components/layout/AccountLayout'
import MainLayout from './components/layout/MainLayout'
import Home from './pages/Home/Home'
import Projects from './pages/Projects/Projects'
import Tasks from './pages/Tasks/Tasks'

function App() {
  const dispatch: AppDispatch = useDispatch();
  const { preferences } = useAppSelector((state) => state.account);

  useEffect(() => {
    if (preferences.darkMode) {
      document.documentElement.classList.toggle("dark");
    } else {
      document.documentElement.classList.remove("dark");
    }
  }, [preferences.darkMode]);

  useEffect(() => {
    const storedUser = localStorage.getItem("user");
    const storedPreferences = localStorage.getItem("preferences");
    if (storedPreferences) {
      try {
        const parsedPreferences = JSON.parse(storedPreferences);
        dispatch(setPreferences(parsedPreferences));
      } catch (error) {
        console.error("Tercih verisi çözümlenirken hata oluştu:", error);
      }
    }
    if (storedUser) {
      try {
        const parsedUser = JSON.parse(storedUser);
        dispatch(setUser(parsedUser));
        dispatch(checkAuth());
      } catch (error) {
        console.error("Kullanıcı verisi çözümlenirken hata oluştu:", error);
      }
    }
  }, [dispatch]);

  return (
    <HistoryRouter history={history}>
      <Routes>
        <Route element={<AccountLayout />} >
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Route>
        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/projects" element={<Projects />} />
            <Route path="/tasks" element={<Tasks />} />
          </Route>
        </Route>
      </Routes>
    </HistoryRouter>
  )
}

export default App
