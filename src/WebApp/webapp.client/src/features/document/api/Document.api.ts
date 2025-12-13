import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { DocumentModel } from '../types/DocumentModel';

const apiURL = '/api/v1';

export const DocumentApi = createApi({
    reducerPath: 'documentApi',
    tagTypes: [
        'Document',
        'Comment',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        createDocument: builder.mutation<void, DocumentModel>({
            query: document => ({
                body: document,
                url: '/Document',
                method: 'POST'
            }),
            invalidatesTags: (result, error, { id }) => [
                { type: "Document", id },
            ]
        }),
        updateDocument: builder.mutation<void, DocumentModel>({
            query: document => ({
                body: document,
                url: `/Document/${document.id}`,
                method: 'PATCH'
            }),
            invalidatesTags: (result, error, { id }) => [
                { type: "Document", id },
            ]
        }),
        deleteDocument: builder.mutation<void, string>({
            query: id => ({
                url: `/Document/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'Document', id }],
        }),
        getAllDocuments: builder.query<DocumentModel[], void>({
            query: () => "/Document",
            providesTags: result =>
                result
                    ? [
                        ...result.map(document => ({ type: 'Document' as const, id: document.id })),
                        { type: 'Document', id: 'LIST' },
                    ]
                    : [{ type: 'Document', id: 'LIST' }],
        }),
        getDocumentByName: builder.query<DocumentModel, string>({
            query: name => `/Document/getByName/${name}`,
            providesTags: result => result ? [{ type: 'Document', id: result.id }] : [],
        }),
        getActualDocumentsByUserId: builder.query<DocumentModel[], { userId: string, page: number, pageSize: number }>({
            query: ({ userId, page, pageSize }) => `/Document/getActualByUserId/${userId}?page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(document => ({ type: 'Document' as const, id: document.id })),
                        { type: 'Document', id: 'LIST' },
                    ]
                    : [{ type: 'Document', id: 'LIST' }],
        }),
        getHistoryDocumentsByUserId: builder.query<DocumentModel[], { userId: string, page: number, pageSize: number }>({
            query: ({ userId, page, pageSize }) => `/Document/getHustoryByUserId/${userId}?page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(document => ({ type: 'Document' as const, id: document.id })),
                        { type: 'Document', id: 'LIST' },
                    ]
                    : [{ type: 'Document', id: 'LIST' }],
        }),
    })
})

export const {
    useCreateDocumentMutation,
    useUpdateDocumentMutation,
    useDeleteDocumentMutation,
    useGetAllDocumentsQuery,
    useLazyGetDocumentByNameQuery,
    useGetActualDocumentsByUserIdQuery,
    useLazyGetActualDocumentsByUserIdQuery,
    useGetHistoryDocumentsByUserIdQuery,
} = DocumentApi;