import { useForm } from 'react-hook-form';
import login from '../../assets/login.png';
import { Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../store/store';
import { clearErrors, loginUser } from './accountSlice';
import { useBreakpoint } from '../../hooks/useBreakpoint';
import { ClipLoader } from 'react-spinners';
import { useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { ErrorDisplay } from '../../components/ui/ErrorDisplay';

export default function Login() {
    const { handleSubmit, register, formState: { errors }, reset } = useForm();
    const dispatch = useAppDispatch();
    const { status, error } = useAppSelector(state => state.account);
    const { down } = useBreakpoint();
    const isMobile = down.sm;
    const [showPassword, setShowPassword] = useState(false);

    const handleLogin = (data: any) => {
        dispatch(loginUser(data));
    }

    useEffect(() => {
        dispatch(clearErrors());
    }, []);

    return (
        <div className="flex items-center justify-center h-screen md:px-20 lg:px-0">
            <div className="sm:flex flex-col md:grid md:grid-cols-2 bg-[rgb(var(--div))]/20 backdrop-filter bg-clip-padding backdrop-blur-md border-[rgb(var(--border))]/6 border rounded-lg shadow-lg lg:w-[60%] lg:h-[80vh] overflow-hidden">
                <div>
                    <form method="POST" onSubmit={handleSubmit(handleLogin)} className="flex flex-col h-full" noValidate>
                        <ErrorDisplay error={error!} />
                        <div className="flex flex-col w-full justify-between p-8 gap-y-1">
                            <div className="flex flex-col">
                                <p className="font-semibold text-center text-2xl">
                                    Hoşgeldiniz
                                </p>
                                <p className="text-gray-400 font-medium text-center">
                                    Lütfen hesabınıza giriş yapınız
                                </p>
                            </div>
                            <div className="w-full mt-auto mb-auto">
                                <div className="flex flex-col gap-y-5 w-full pt-4">
                                    <div className="flex flex-col gap-y-2">
                                        <label htmlFor="email" className="font-semibold">E-Posta</label>
                                        <input type="email" id="email"{...register("email", {
                                            required: "E-Posta zorunludur.",
                                            pattern: {
                                                value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
                                                message: "Geçersiz e-posta adresi."
                                            }
                                        })} placeholder="E-Postanızı Giriniz" className="border-gray-400 border rounded-lg p-2 outline-none w-full transition-all duration-500 focus:scale-[103%] focus:border-black" />
                                        {errors.email && <p className="text-red-500 text-sm">{errors.email.message?.toString()}</p>}
                                    </div>
                                    <div className="flex flex-col gap-y-2 mb-2">
                                        <label htmlFor="password" className="font-semibold">Şifre</label>
                                        <div className="flex justify-between">
                                            <input type={showPassword ? "text" : "password"} id="password"{...register("password", {
                                                required: "Şifre zorunludur.",
                                                minLength: {
                                                    value: 6,
                                                    message: "Şifre en az 6 karakter olmalıdır."
                                                }
                                            })} placeholder="Şifrenizi giriniz" className="border-gray-400 border-t border-l border-b border-r-0 rounded-tl-lg rounded-bl-lg p-2 outline-none w-full transition-all duration-500 focus:scale-[103%] focus:border-black" />
                                            <button type="button" onClick={() => setShowPassword(!showPassword)} title={showPassword ? "Gizle" : "Göster"} className="w-12 cursor-pointer rounded-br-lg rounded-tr-lg border border-gray-400 hover:scale-[103%] hover:bg-gray-100 transition-transform duration-500">
                                                <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} />
                                            </button>
                                        </div>
                                        {errors.password && <p className="text-red-500 text-sm">{errors.password.message?.toString()}</p>}
                                    </div>
                                    <div className="flex flex-row gap-x-2">
                                        <input type="checkbox" id="rememberMe" {...register("rememberMe")} className="w-4" />
                                        <label htmlFor="rememberMe" className="font-medium">Beni Hatırla</label>
                                    </div>
                                </div>
                                <div className="flex flex-col gap-y-12">
                                    <button type="submit" disabled={status == "pending"} className="mt-5 w-full bg-[rgb(var(--btn-primary))] cursor-pointer font-medium shadow-lg border border-white/6 text-white p-2 rounded-lg hover:bg-gray-800 hover:scale-[103%] transition-all duration-500">
                                        {status == "pending" ? (
                                            <ClipLoader size={20} className="justify-center align-middle text-center" color="#fff" />
                                        ) : (
                                            "Giriş Yap"
                                        )}
                                    </button>
                                    <div className="flex flex-col text-center">
                                        <p className="text-center text-gray-500">Hesabınız mı yok?</p>
                                        <Link to="/register" className="text-center text-gray-500 underline">Kayıt Ol</Link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                {!isMobile && (
                    <div className="overflow-hidden">
                        <img src={login} alt="Task MNG" className="w-full h-full object-fill" />
                    </div>
                )}

            </div>
        </div>
    );
}