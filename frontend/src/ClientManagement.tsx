import React, { useState, useEffect, ChangeEvent } from 'react';
import axios from 'axios';

interface Client {
    id?: number;
    clientId: string;
    secret: string;
    redirectUri: string;
}

const ClientManagement: React.FC = () => {
    const [clients, setClients] = useState<Client[]>([]);
    const [client, setClient] = useState<Client>({ clientId: '', secret: '', redirectUri: '' });

    useEffect(() => {
        axios.get<Client[]>('/api/clients')
            .then(response => setClients(response.data))
            .catch(error => console.error(error));
    }, []);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setClient(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = () => {
        axios.post('/api/clients', client)
            .then(response => {
                setClients([...clients, response.data]);
                setClient({ clientId: '', secret: '', redirectUri: '' });
            })
            .catch(error => console.error(error));
    };

    return (
        <div>
            <h2>Client Management</h2>
            <input type="text" name="clientId" value={client.clientId} onChange={handleChange} placeholder="Client ID" />
            <input type="text" name="secret" value={client.secret} onChange={handleChange} placeholder="Secret" />
            <input type="text" name="redirectUri" value={client.redirectUri} onChange={handleChange} placeholder="Redirect URI" />
            <button onClick={handleSubmit}>Add Client</button>
            
            <h3>Existing Clients</h3>
            <ul>
                {clients.map(c => (
                    <li key={c.clientId}>{c.clientId}</li>
                ))}
            </ul>
        </div>
    );
};

export default ClientManagement;
