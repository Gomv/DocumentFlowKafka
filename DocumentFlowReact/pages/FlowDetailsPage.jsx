import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { api } from '../api/api.js';

export default function FlowDetailsPage() {
    const { id } = useParams();
    const [flow, setFlow] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        async function load() {
            try {
                const result = await api.getFlow(id);
                setFlow(result.flow);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }

        load();
    }, [id]);

    if (loading) return <div className="page">Загрузка...</div>;
    if (error) return <div className="page error">{error}</div>;
    if (!flow) return <div className="page">Документооборот не найден.</div>;

    return (
        <div className="page">
            <Link to="/flows">← К списку</Link>

            <h1>{flow.name}</h1>

            <div className="card">
                <p><strong>Id:</strong> {flow.id}</p>
                <p>
                    <strong>Создан:</strong>{' '}
                    {new Date(flow.dateCreate).toLocaleString()}
                </p>
            </div>

            <h2>Документы</h2>
            <p>
                Здесь дальше добавим список документов и форму добавления
                нового документа.
            </p>
        </div>
    );
}