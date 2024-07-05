import axios, { InternalAxiosRequestConfig } from 'axios';
import { User } from 'oidc-client';
import config from './config';
import userManager from './userManager';

const apiClient = axios.create({
    baseURL: config.apiBaseUrl,
});

apiClient.interceptors.request.use(async (config: InternalAxiosRequestConfig) => {
    const user: User | null = await userManager.getUser();
    if (user && user.access_token) {
        config.headers = {
            ...config.headers,
            Authorization: `Bearer ${user.access_token}`,
        } as any; // Cast to any to avoid type mismatch
    }
    return config;
}, (error) => {
    return Promise.reject(error);
});

export default apiClient;
