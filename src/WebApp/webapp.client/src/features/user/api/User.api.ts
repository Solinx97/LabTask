import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { UserModel } from '../types/UserModel';
import type { LoginModel } from '../types/LoginModel';
import type { RegistrationModel } from '../types/RegistrationModel';

const apiURL = '/api/v1';

export const UserApi = createApi({
    reducerPath: 'userApi',
    tagTypes: [
        'User',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        registration: builder.mutation<void, RegistrationModel>({
            query: user => ({
                body: user,
                url: '/User/register',
                method: 'POST'
            }),
        }),
        login: builder.mutation<UserModel, LoginModel>({
            query: user => ({
                body: user,
                url: '/User/login',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'User', id: result.id }] : [],
        }),
        logout: builder.mutation<void, void>({
            query: () => ({
                url: '/User/logout',
                method: 'POST'
            }),
        }),
    })
})

export const {
    useRegistrationMutation,
    useLoginMutation,
    useLogoutMutation,
} = UserApi;