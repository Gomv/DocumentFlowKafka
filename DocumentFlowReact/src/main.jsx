import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './auth/AuthContext.jsx';
import LoginPage from './pages/LoginPage.jsx';
import FlowsPage from './pages/FlowsPage.jsx';
import FlowDetailsPage from './pages/FlowDetailsPage.jsx';
import './index.css';

function RequireAuth({ children }) {
    const { token } = useAuth();

    if (!token) {
        return <Navigate to="/login" replace />;
    }

    return children;
}

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <BrowserRouter>
            <AuthProvider>
                <Routes>
                    <Route path="/login" element={<LoginPage />} />

                    <Route
                        path="/flows"
                        element={
                            <RequireAuth>
                                <FlowsPage />
                            </RequireAuth>
                        }
                    />

                    <Route
                        path="/flows/:id"
                        element={
                            <RequireAuth>
                                <FlowDetailsPage />
                            </RequireAuth>
                        }
                    />

                    <Route path="*" element={<Navigate to="/flows" replace />} />
                </Routes>
            </AuthProvider>
        </BrowserRouter>
    </StrictMode>
);