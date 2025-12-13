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
        users: builder.query<UserModel[], void>({
            query: () => '/User',
            providesTags: result =>
                result
                    ? [
                        ...result.map(user => ({ type: 'User' as const, id: user.id })),
                        { type: 'User', id: 'LIST' },
                    ]
                    : [{ type: 'User', id: 'LIST' }],
        }),
        getUserById: builder.query<UserModel, string>({
            query: userId => `/User/${userId}`,
            providesTags: result => result ? [{ type: 'User', id: result.id }] : [],
        }),
        refresh: builder.query<UserModel, void>({
            query: () => '/User/refresh',
            providesTags: result => result ? [{ type: 'User', id: result.id }] : [],
        }),
    })
})

export const {
    useRegistrationMutation,
    useLoginMutation,
    useLogoutMutation,
    useUsersQuery,
    useGetUserByIdQuery,
    useLazyRefreshQuery,
} = UserApi;