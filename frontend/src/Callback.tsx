import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import userManager from './userManager';

const Callback: React.FC = () => {
    const location = useLocation();

    useEffect(() => {
        userManager.signinRedirectCallback().then(() => {
            window.location.href = '/';
        });
    }, [location]);

    return <div>Loading...</div>;
};

export default Callback;
