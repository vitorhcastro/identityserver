import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import ClientManagement from './ClientManagement';
import ApiResourceManagement from './ApiResourceManagement';
import ApiScopeManagement from './ApiScopeManagement';
import Login from './Login';
import Home from './Home';
import './App.css';

const App: React.FC = () => {
    return (
        <Router>
            <div className="app-container">
                <nav>
                    <ul>
                        <li>
                            <Link to="/">Home</Link>
                        </li>
                        <li>
                            <Link to="/clients">Client Management</Link>
                        </li>
                        <li>
                            <Link to="/apiresources">API Resource Management</Link>
                        </li>
                        <li>
                            <Link to="/apiscopes">API Scope Management</Link>
                        </li>
                        <li>
                            <Link to="/login">Login</Link>
                        </li>
                    </ul>
                </nav>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/clients" element={<ClientManagement />} />
                    <Route path="/apiresources" element={<ApiResourceManagement />} />
                    <Route path="/apiscopes" element={<ApiScopeManagement />} />
                    <Route path="/login" element={<Login />} />
                </Routes>
            </div>
        </Router>
    );
};

export default App;
