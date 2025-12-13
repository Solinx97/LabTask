import type { LinkModel } from '../types/LinkModel';
import { DocumentApi } from './Document.api';

export const LinkApi = DocumentApi.injectEndpoints({
    endpoints: builder => ({
        createLink: builder.mutation<LinkModel, LinkModel>({
            query: link => ({
                body: link,
                url: '/Link',
                method: 'POST'
            }),
            async onQueryStarted(link, { dispatch, queryFulfilled }) {
                const patches = [
                    dispatch(
                        LinkApi.util.updateQueryData(
                            'getLinksByUserId',
                            { userId: link.toUserId },
                            draft => {
                                draft.unshift(link);
                            }
                        )
                    ),
                    dispatch(
                        LinkApi.util.updateQueryData(
                            'getLinksByOwnerId',
                            { userId: link.ownerId },
                            draft => {
                                draft.unshift(link);
                            }
                        )
                    ),
                ];

                try {
                    const { data: created } = await queryFulfilled;

                    dispatch(
                        LinkApi.util.updateQueryData(
                            'getLinksByUserId',
                            { userId: created.toUserId },
                            draft => {
                                const index = draft.findIndex(l => l.id === link.id);
                                if (index !== -1) {
                                    draft[index] = created;
                                }
                            }
                        )
                    );
                    dispatch(
                        LinkApi.util.updateQueryData(
                            'getLinksByOwnerId',
                            { userId: created.ownerId },
                            draft => {
                                const index = draft.findIndex(l => l.id === link.id);
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
        deleteLink: builder.mutation<void, { id: string, userId: string }>({
            query: ({ id }) => ({
                url: `/Link/${id}`,
                method: 'DELETE'
            }),
            async onQueryStarted({ id, userId }, { dispatch, queryFulfilled }) {
                const patches = [
                    dispatch(
                        LinkApi.util.updateQueryData(
                            'getLinksByUserId',
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
                        LinkApi.util.updateQueryData(
                            'getLinksByOwnerId',
                            { userId },
                            draft => {
                                const index = draft.findIndex(d => d.id === id);
                                if (index !== -1) {
                                    draft.splice(index, 1);
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
        getLinksByUserId: builder.query<LinkModel[], { userId: string, page: number, pageSize: number }>({
            query: ({ userId, page, pageSize }) => `/Link/getByUserId/${userId}?page=${page}&pageSize=${pageSize}`,
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
                        ...result.map(link => ({ type: 'Link' as const, id: link.id })),
                        { type: 'Link', id: 'LIST' },
                    ]
                    : [{ type: 'Link', id: 'LIST' }],
        }),
        getLinksByOwnerId: builder.query<LinkModel[], { userId: string, page: number, pageSize: number }>({
            query: ({ userId, page, pageSize }) => `/Link/getByOwnerId/${userId}?page=${page}&pageSize=${pageSize}`,
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
                        ...result.map(link => ({ type: 'Link' as const, id: link.id })),
                        { type: 'Link', id: 'LIST' },
                    ]
                    : [{ type: 'Link', id: 'LIST' }],
        }),
    })
})

export const {
    useCreateLinkMutation,
    useDeleteLinkMutation,
    useGetLinksByUserIdQuery,
    useGetLinksByOwnerIdQuery,
} = LinkApi;