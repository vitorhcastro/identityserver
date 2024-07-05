import React, { useEffect } from 'react';
import { useAuth } from './auth';

const Logout: React.FC = () => {
    const { isAuthenticated, signOut } = useAuth();

    useEffect(() => {
        signOut();
    }, [isAuthenticated, signOut]);


    return <div>Redirecting to logout...</div>;
};

export default Logout;
