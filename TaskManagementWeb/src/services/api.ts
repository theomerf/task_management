import axios, { AxiosError, type InternalAxiosRequestConfig } from 'axios';
import { toast } from 'react-toastify';
import { store } from '../store/store';
import { logout, setUser } from '../pages/Account/accountSlice';
import type { ApiErrorResponse } from '../types/apiError';
import { history } from '../utils/history';

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

const AUTH_ENDPOINTS = ['account/login', 'account/refresh', 'account/check-auth'];

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
                console.log(originalRequest.url);
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
        404: data?.message ?? "İçerik bulunamadı",
        500: data?.message ?? "Sunucu hatası"
    };

    if (status === 400 || status === 422) {
        if (data?.message) toast.error(data.message);
        return;
    }

    const message = errorMessages[status];
    if (message) toast.error(message);

    if (status === 403 || status === 404)
        history.push('/');
};

const methods = {
    get: (url: string, params?: any, signal?: AbortSignal) => axios.get(url, { ...params, signal }).then((response) => ({ data: response.data, headers: response.headers })),
    getWithoutHeaders: (url: string, params?: any, signal?: AbortSignal) => axios.get(url, { ...params, signal }).then((response) => response.data),
    post: (url: string, body: any | null, config?: any | null) => axios.post(url, body, config).then((response) => response.data),
    put: (url: string, body: any, config?: any | null) => axios.put(url, body, config).then((response) => response.data),
    patch: (url: string, body?: any, config?: any | null) => axios.patch(url, body, config).then((response) => response.data),
    delete: (url: string) => axios.delete(url).then((response) => response.data),
};

const account = {
    login: (formData: any) => methods.post("account/login", formData),
    register: (formData: any) => methods.post("account/register", formData),
    refresh: () => methods.post("account/refresh", null, { headers: { "X-No-Retry": "true" } }),
    logout: () => methods.post("account/logout", null),
    checkAuth: (signal?: AbortSignal) => methods.getWithoutHeaders("account/check-auth", {}, signal),
};

const project = {
    getAll: (params: any, signal?: AbortSignal) => methods.get("project", params, signal),
    getOne: (id: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${id}`, {}, signal),
    me: (signal?: AbortSignal) => methods.getWithoutHeaders("project/me", {}, signal),
    create: (formData: any) => methods.post("project/create", formData),
    update: (formData: any) => methods.put(`project/update`, formData),
    delete: (id: string) => methods.delete(`project/delete/${id}`),
    restore: (id: string) => methods.patch(`project/restore/${id}`),
    getSettings: (projectId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/settings`, {}, signal),
    updateSettings: (projectId: string, formData: any) => methods.put(`project/${projectId}/settings/update`, formData),
    getLabels: (projectId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/labels`, {}, signal),
    getOneLabel: (projectId: string, labelId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/labels/${labelId}`, {}, signal),
    createLabel: (projectId: string, formData: any) => methods.post(`project/${projectId}/labels/create`, formData),
    updateLabel: (projectId: string, formData: any) => methods.put(`project/${projectId}/labels/update`, formData),
    deleteLabel: (projectId: string, labelId: string) => methods.delete(`project/${projectId}/labels/delete/${labelId}`),
    getMembers: (projectId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/members`, {}, signal),
    getOneMember: (projectId: string, memberId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/members/${memberId}`, {}, signal),
    addMember: (projectId: string, formData: any) => methods.post(`project/${projectId}/members/add`, formData),
    updateMember: (projectId: string, formData: any) => methods.put(`project/${projectId}/members/update`, formData),
    removeMember: (projectId: string, memberId: string) => methods.delete(`project/${projectId}/members/remove/${memberId}`),
};

const task = {
    getAll: (projectId: string, params: any, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/task`, params, signal),
    getOne: (projectId: string, taskId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/task/${taskId}`, {}, signal),
    create: (projectId: string, formData: any) => methods.post(`project/${projectId}/task/create`, formData),
    update: (projectId: string, formData: any) => methods.put(`project/${projectId}/task/update`, formData),
    updateStatus: (projectId: string, formData: any) => methods.patch(`project/${projectId}/task/update-status`, formData),
    updatePriority: (projectId: string, formData: any) => methods.patch(`project/${projectId}/task/update-priority`, formData),
    updateLabel: (projectId: string, formData: any) => methods.patch(`project/${projectId}/task/update-label`, formData),
    delete: (projectId: string, taskId: string) => methods.delete(`project/${projectId}/task/delete/${taskId}`),
    getAttachments: (projectId: string, taskId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/task/${taskId}/attachment`, {}, signal),
    getOneAttachment: (projectId: string, taskId: string, attachmentId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/task/${taskId}/attachment/${attachmentId}`, {}, signal),
    createAttachment: (projectId: string, formData: any) => methods.post(`project/${projectId}/task//attachment/create`, formData),
    updateAttachment: (projectId: string, formData: any) => methods.put(`project/${projectId}/task/attachment/update`, formData),
    deleteAttachment: (projectId: string, taskId: string, attachmentId: string) => methods.delete(`project/${projectId}/task/${taskId}/attachment/delete/${attachmentId}`),
    getTimeLogs: (projectId: string, taskId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/task/${taskId}/timelog`, {}, signal),
    getOneTimeLog: (projectId: string, taskId: string, timeLogId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/task/${taskId}/timelog/${timeLogId}`, {}, signal),
    createTimeLog: (projectId: string, formData: any) => methods.post(`project/${projectId}/task/timelog/create`, formData),
    updateTimeLog: (projectId: string, formData: any) => methods.put(`project/${projectId}/task/timelog/update`, formData),
    deleteTimeLog: (projectId: string, taskId: string, timeLogId: string) => methods.delete(`project/${projectId}/task/${taskId}/timelog/delete/${timeLogId}`),
    getTimeLogCategories: (projectId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/timelog-category`, {}, signal),
    getOneTimeLogCategory: (projectId: string, categoryId: string, signal?: AbortSignal) => methods.getWithoutHeaders(`project/${projectId}/timelog-category/${categoryId}`, {}, signal),
    createTimeLogCategory: (projectId: string, formData: any) => methods.post(`project/${projectId}/timelog-category/create`, formData),
    updateTimeLogCategory: (projectId: string, formData: any) => methods.put(`project/${projectId}/timelog-category/update`, formData),
    deleteTimeLogCategory: (projectId: string, categoryId: string) => methods.delete(`project/${projectId}/timelog-category/delete/${categoryId}`),
}

const errors = {
    get400Error: () => methods.get("errors/bad-request"),
    get401Error: () => methods.get("errors/unauthorized"),
    get403Error: () => methods.get("errors/validation-error"),
    get404Error: () => methods.get("errors/not-found"),
    get500Error: () => methods.get("errors/server-error"),
};

const requests = {
    account,
    project,
    task,
    errors,
};

export default requests;