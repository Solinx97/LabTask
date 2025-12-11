import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { DocumentModel } from '../types/DocumentModel';

const apiURL = '/api/v1';

export const DocumentApi = createApi({
    reducerPath: 'documentApi',
    tagTypes: [
        'Document',
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
                        ...result.map(docuemnt => ({ type: 'Document' as const, id: docuemnt.id })),
                        { type: 'Document', id: 'LIST' },
                    ]
                    : [{ type: 'Document', id: 'LIST' }],
        }),
        getDocumentById: builder.query<DocumentModel, string>({
            query: id => `/Document/${id}`,
            providesTags: result => result ? [{ type: 'Document', id: result.id }] : [],
        }),
        getDocumentsByUserId: builder.query<DocumentModel[], string>({
            query: userId => `/Document/getByUserId/${userId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(docuemnt => ({ type: 'Document' as const, id: docuemnt.id })),
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
    useLazyGetDocumentByIdQuery,
    useGetDocumentsByUserIdQuery,
} = DocumentApi;