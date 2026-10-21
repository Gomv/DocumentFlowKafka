import { createContext, useContext, useState } from 'react';
import { api, setToken, clearToken, getToken } from '../api/api.js';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
    const [token, setTokenState] = useState(getToken());

    async function login(uid) {
        const result = await api.login(uid);
        setToken(result.token);
        setTokenState(result.token);
    }

    function logout() {
        clearToken();
        setTokenState(null);
    }

    return (
        <AuthContext.Provider value={{ token, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    return useContext(AuthContext);
}