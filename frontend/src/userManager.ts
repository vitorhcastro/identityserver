import { UserManager, WebStorageStateStore } from 'oidc-client';

const settings = {
    authority: 'https://localhost:7196',
    client_id: 'react-client',
    redirect_uri: 'http://localhost:3000/callback',
    response_type: 'code',
    scope: 'openid profile api1',
    post_logout_redirect_uri: 'http://localhost:3000/',
    userStore: new WebStorageStateStore({ store: window.localStorage })
};

const userManager = new UserManager(settings);

export default userManager;
