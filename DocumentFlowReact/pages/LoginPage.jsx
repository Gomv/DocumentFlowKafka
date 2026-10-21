import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext.jsx';

export default function LoginPage() {
    const [uid, setUid] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();

    async function handleSubmit(event) {
        event.preventDefault();
        setError('');
        setLoading(true);

        try {
            await login(uid);
            navigate('/flows');
        } catch (err) {
            setError('Не удалось войти. Проверь UId.');
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="page">
            <h1>Вход в документооборот</h1>

            <form onSubmit={handleSubmit} className="card">
                <label>
                    UId пользователя
                    <input
                        value={uid}
                        onChange={(e) => setUid(e.target.value)}
                        placeholder="Вставь GUID пользователя"
                        required
                    />
                </label>

                <button type="submit" disabled={loading}>
                    {loading ? 'Входим...' : 'Войти'}
                </button>

                {error && <p className="error">{error}</p>}
            </form>
        </div>
    );
}