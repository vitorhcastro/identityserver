import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Log } from 'oidc-client';
import userManager from './userManager';

Log.logger = console;
Log.level = Log.DEBUG;

const Callback: React.FC = () => {
    const navigate = useNavigate();

    useEffect(() => {
        userManager.signinRedirectCallback().then((user) => {
            if (user) {
                console.log('User:', user);
                navigate('/');
            }
        }).catch(error => {
            console.error('Error during callback handling:', error);
            navigate('/'); // Redirect home even if there's an error
        });
    }, [navigate]);

    return <div>Loading...</div>;
};

export default Callback;
