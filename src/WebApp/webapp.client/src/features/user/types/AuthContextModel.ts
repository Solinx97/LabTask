import type { LoginModel } from "./LoginModel";

export type AuthContextModel = {
    isAuthenticated: boolean;
    authInProgress: boolean;
    checkAuthAsync: () => Promise<void>;
    loginAsync: (loginUser: LoginModel) => Promise<void>;
    logoutAsync: () => Promise<void>;
}