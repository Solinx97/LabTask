import { fileURLToPath, URL } from 'node:url';

import { defineConfig, loadEnv } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import { env } from 'process';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    const apiVersion = env.VITE_API_VERSION ? env.VITE_API_VERSION : 'v1';
    const target = env.VITE_APP_SERVER_PROXY_URL || 'http://localhost:5120';

    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            }
        },
        server: {
            proxy: {
                [`^/api/${apiVersion}/User`]: { target, secure: false },
                [`^/api/${apiVersion}/Document`]: { target, secure: false },
            },
            port: parseInt(env.DEV_SERVER_PORT || '63149'),
        }
    }
})
