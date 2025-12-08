import axios, { AxiosError, type InternalAxiosRequestConfig } from 'axios';
import { toast } from 'react-toastify';
import { store } from '../store/store';
import { logout, setUser } from '../pages/Account/accountSlice';
import type { ApiErrorResponse } from '../types/apiError';

const apiBase = (import.meta.env.VITE_API_BASE_URL as string | undefined)?.replace(/\/+$/, '') || 'https://localhost:7118';

axios.defaults.withCredentials = true;
axios.defaults.headers.common['Content-Type'] = 'application/json';
axios.defaults.baseURL = `${apiBase}/api/`;

interface QueueItem {
    resolve: (value?: any) => void;
    reject: (reason?: any) => void;
}

class TokenRefreshManager {
    private isRefreshing = false;
    private failedQueue: QueueItem[] = [];

    private processQueue(error: any = null, token: any = null) {
        this.failedQueue.forEach((promise) => {
            error ? promise.reject(error) : promise.resolve(token);
        });
        
        this.failedQueue = [];
        this.isRefreshing = false;
    }

    async handleTokenRefresh(originalRequest: InternalAxiosRequestConfig): Promise<any> {
        if (this.isRefreshing) {
            return new Promise((resolve, reject) => {
                this.failedQueue.push({ resolve, reject });
            }).then(() => axios(originalRequest));
        }

        this.isRefreshing = true;

        try {
            const response = await account.refresh();
            store.dispatch(setUser(response));
            this.processQueue(null, response);
            return axios(originalRequest);
        } catch (error) {
            this.processQueue(error);
            store.dispatch(logout());
            throw error;
        }
    }
}

const tokenManager = new TokenRefreshManager();

const AUTH_ENDPOINTS = ['/account/login', '/account/refresh', '/account/check-auth'];

const shouldSkipRetry = (url?: string): boolean => {
    return AUTH_ENDPOINTS.some(endpoint => url?.includes(endpoint));
};

axios.interceptors.response.use(
    (response) => response,
    async (error: AxiosError<ApiErrorResponse>) => {
        if (!error.response || error.code === "ERR_CANCELED") {
            if (!error.code?.includes("CANCELED")) {
                toast.error("Sunucuya ulaşılamıyor");
            }
            return Promise.reject(error);
        }

        const { status, config } = error.response;
        const originalRequest = config as InternalAxiosRequestConfig & { _retry?: boolean };

        if (status === 401 && !shouldSkipRetry(originalRequest.url) && !originalRequest._retry) {
            originalRequest._retry = true;
            return tokenManager.handleTokenRefresh(originalRequest);
        }

        handleErrorByStatus(status, error.response.data);

        return Promise.reject(error);
    }
);

function handleErrorByStatus(status: number, data?: ApiErrorResponse) {
    const errorMessages: Record<number, string> = {
        403: "Bu işlem için yetkiniz yok",
        404: data?.message ?? "Kaynak bulunamadı",
        500: data?.message ?? "Sunucu hatası"
    };

    if (status === 400 || status === 422) {
        if (data?.message) toast.error(data.message);
        return;
    }

    const message = errorMessages[status];
    if (message) toast.error(message);
}

const methods = {
    get: (url: string, params?: any, signal?: AbortSignal) => axios.get(url, { ...params, signal }).then((response) => ({ data: response.data, headers: response.headers })),
    getWithoutHeaders: (url: string, params?: any, signal?: AbortSignal) => axios.get(url, { ...params, signal }).then((response) => response.data),
    post: (url: string, body: any | null, config?: any | null) => axios.post(url, body, config).then((response) => response.data),
    put: (url: string, body: any, config?: any | null) => axios.put(url, body, config).then((response) => response.data),
    patch: (url: string, body: any, config?: any | null) => axios.patch(url, body, config).then((response) => response.data),
    delete: (url: string) => axios.delete(url).then((response) => response.data),
};

const account = {
    login: (formData: any) => methods.post("account/login", formData),
    register: (formData: any) => methods.post("account/register", formData),
    refresh: () => methods.post("account/refresh", null, { headers: { "X-No-Retry": "true" } }),
    logout: () => methods.post("account/logout", null),
    checkAuth: (signal?: AbortSignal) => methods.getWithoutHeaders("account/check-auth", {}, signal),
};

const errors = {
    get400Error: () => methods.get("errors/bad-request"),
    get401Error: () => methods.get("errors/unauthorized"),
    get403Error: () => methods.get("errors/validation-error"),
    get404Error: () => methods.get("errors/not-found"),
    get500Error: () => methods.get("errors/server-error"),
};

const requests = {
    account,
    errors,
};

export default requests;