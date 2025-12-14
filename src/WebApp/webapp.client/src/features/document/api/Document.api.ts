import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { DocumentModel } from '../types/DocumentModel';
import type { StatisticModel } from '../types/StatisticModel';

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
        createDocument: builder.mutation<DocumentModel, DocumentModel>({
            query: document => ({
                body: document,
                url: '/Document',
                method: 'POST'
            }),
            async onQueryStarted(document, { dispatch, queryFulfilled }) {
                const patches = [
                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getActualDocumentsByUserId',
                            { userId: document.userId },
                            draft => {
                                draft.unshift(document);
                            }
                        )
                    ),
                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getHistoryDocumentsByUserId',
                            { userId: document.userId },
                            draft => {
                                draft.unshift(document);
                            }
                        )
                    ),
                ];

                try {
                    const { data: created } = await queryFulfilled;

                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getActualDocumentsByUserId',
                            { userId: created.userId },
                            draft => {
                                const index = draft.findIndex(l => l.id === document.id);
                                if (index !== -1) {
                                    draft[index] = created;
                                }
                            }
                        )
                    );
                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getHistoryDocumentsByUserId',
                            { userId: created.userId },
                            draft => {
                                const index = draft.findIndex(l => l.id === document.id);
                                if (index !== -1) {
                                    draft[index] = created;
                                }
                            }
                        )
                    );
                } catch {
                    patches.forEach(p => p.undo());
                }
            },
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
        deleteDocument: builder.mutation<void, { id: string; userId: string }>({
            query: ({ id }) => ({
                url: `/Document/${id}`,
                method: 'DELETE'
            }),
            async onQueryStarted({ id, userId }, { dispatch, queryFulfilled }) {
                const patches = [
                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getActualDocumentsByUserId',
                            { userId },
                            draft => {
                                const index = draft.findIndex(d => d.id === id);
                                if (index !== -1) {
                                    draft.splice(index, 1);
                                }
                            }
                        )
                    ),
                    dispatch(
                        DocumentApi.util.updateQueryData(
                            'getDocumentById',
                            id,
                            () => undefined
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
        getDocumentByName: builder.query<DocumentModel[], { userId: string, name: string }>({
            query: ({ userId, name }) => `/Document/getByName/${userId}?name=${name}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(document => ({ type: 'Document' as const, id: document.id })),
                        { type: 'Document', id: 'LIST' },
                    ]
                    : [{ type: 'Document', id: 'LIST' }],
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
        getExpiredDocumentsByUserId: builder.query<DocumentModel[], { userId: string, page: number, pageSize: number }>({
            query: ({ userId, page, pageSize }) => `/Document/getExpiredDocumentsByUserId/${userId}?page=${page}&pageSize=${pageSize}`,
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
        getDocumentStatisticsByYear: builder.query<StatisticModel[], string>({
            query: (userId) => `/Document/statisticsByYear/${userId}`,
        }),
        getDocumentStatisticsByRange: builder.query<StatisticModel[], { userId: string, startedAt: string, finishedAt: string }>({
            query: ({ userId, startedAt, finishedAt }) => `/Document/statisticsByYear/${userId}?startedAt${startedAt}&finishedAt=${finishedAt}`,
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
    useGetExpiredDocumentsByUserIdQuery,
    useGetDocumentStatisticsByYearQuery,
    useLazyGetDocumentStatisticsByRangeQuery,
} = DocumentApi;