import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import fs from 'fs'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    tailwindcss()
  ],
  server: {
    https: {
      pfx: fs.readFileSync('cert/localhost.pfx'),
      passphrase: 'dev123',
    },
    host: 'localhost',
    port: 3000,
  }
})
