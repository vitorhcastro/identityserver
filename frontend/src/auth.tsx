import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User, Log } from 'oidc-client';
import userManager from './userManager';

Log.logger = console;
Log.level = Log.DEBUG;

interface AuthContextProps {
    isAuthenticated: boolean;
    user: User | null;
    signIn: () => void;
    signOut: () => void;
}

interface AuthProviderProps {
    children: ReactNode;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        userManager.getUser().then((loadedUser) => {
            setUser(loadedUser);
        });

        userManager.events.addUserLoaded((loadedUser) => {
            setUser(loadedUser);
        });

        userManager.events.addUserUnloaded(() => {
            setUser(null);
        });

        return () => {
            userManager.events.removeUserLoaded((loadedUser) => {
                setUser(loadedUser);
            });

            userManager.events.removeUserUnloaded(() => {
                setUser(null);
            });
        };
    }, []);

    const signIn = () => {
        userManager.signinRedirect();
    };

    const signOut = () => {
        userManager.signoutRedirect();
    };

    const isAuthenticated = user !== null;

    return (
        <AuthContext.Provider value={{ isAuthenticated, user, signIn, signOut }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
