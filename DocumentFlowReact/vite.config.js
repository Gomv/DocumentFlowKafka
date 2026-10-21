import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 49862,
        proxy: {
            '/api': {
                target: 'https://localhost:5270', // порт твоего ASP.NET API
                changeOrigin: true,
                secure: false
            }
        }
    }
})