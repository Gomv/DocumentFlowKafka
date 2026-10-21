import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { api } from '../api/api.js';
import { useAuth } from '../auth/AuthContext.jsx';

export default function FlowsPage() {
    const [flows, setFlows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const { logout } = useAuth();

    useEffect(() => {
        async function load() {
            try {
                const result = await api.getFlows();
                setFlows(result.flows ?? []);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }

        load();
    }, []);

    return (
        <div className="page">
            <div className="page-header">
                <h1>Документообороты</h1>
                <button onClick={logout}>Выйти</button>
            </div>

            {loading && <p>Загрузка...</p>}
            {error && <p className="error">{error}</p>}

            {!loading && flows.length === 0 && (
                <p>Документооборотов пока нет.</p>
            )}

            <div className="flow-list">
                {flows.map((flow) => (
                    <Link
                        key={flow.id}
                        to={`/flows/${flow.id}`}
                        className="card flow-card"
                    >
                        <h2>{flow.name}</h2>
                        <p>Создан: {new Date(flow.dateCreate).toLocaleString()}</p>
                    </Link>
                ))}
            </div>
        </div>
    );
}