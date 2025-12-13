import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { DocumentModel } from '../types/DocumentModel';

const apiURL = '/api/v1';

export const DocumentApi = createApi({
    reducerPath: 'documentApi',
    tagTypes: [
        'Document',
        'Comment',
        'Link',
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
            async onQueryStarted(document, { dispatch, queryFulfilled }) {
                const patches = [
                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getActualDocumentsByUserId',
                            { userId: document.userId },
                            draft => {
                                const doc = draft.find(d => d.id === document.id);
                                if (doc) {
                                    Object.assign(doc, document);
                                }
                            }
                        )
                    ),
                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getDocumentById',
                            document.id,
                            draft => {
                                if (draft) {
                                    Object.assign(draft, document);
                                }
                            }
                        )
                    ),
                ];
                try {
                    await queryFulfilled;
                } catch {
                    patches.forEach(p => p.undo());
                }
            }
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
        getDocumentById: builder.query<DocumentModel, string>({
            query: id => `/Document/${id}`,
            providesTags: result => result ? [{ type: 'Document', id: result.id }] : [],
        }),
        getDocumentByName: builder.query<DocumentModel, string>({
            query: name => `/Document/getByName/${name}`,
            providesTags: result => result ? [{ type: 'Document', id: result.id }] : [],
        }),
        getActualDocumentsByUserId: builder.query<DocumentModel[], { userId: string, page: number, pageSize: number }>({
            query: ({ userId, page, pageSize }) => `/Document/getActualByUserId/${userId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.userId}`,
            merge: (currentCache, newItems) => {
                newItems.forEach(item => {
                    const index = currentCache.findIndex(d => d.id === item.id);
                    if (index === -1) {
                        currentCache.push(item);
                    } else {
                        currentCache[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => currentArg?.page !== previousArg?.page,
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
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.userId}`,
            merge: (currentCache, newItems) => {
                newItems.forEach(item => {
                    const index = currentCache.findIndex(d => d.id === item.id);
                    if (index === -1) {
                        currentCache.push(item);
                    } else {
                        currentCache[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => currentArg?.page !== previousArg?.page,
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
    useGetDocumentByIdQuery,
    useLazyGetDocumentByNameQuery,
    useGetActualDocumentsByUserIdQuery,
    useLazyGetActualDocumentsByUserIdQuery,
    useGetHistoryDocumentsByUserIdQuery,
} = DocumentApi;