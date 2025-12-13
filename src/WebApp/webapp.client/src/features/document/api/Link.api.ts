import type { LinkModel } from '../types/LinkModel';
import { DocumentApi } from './Document.api';

export const LinkApi = DocumentApi.injectEndpoints({
    endpoints: builder => ({
        createLink: builder.mutation<void, LinkModel>({
            query: link => ({
                body: link,
                url: '/Link',
                method: 'POST'
            }),
            invalidatesTags: (result, error, { id }) => [
                { type: "Link", id },
            ]
        }),
        deleteLink: builder.mutation<void, { documentId: string, LinkId: string }>({
            query: ({ documentId, LinkId: linkId }) => ({
                url: `/Link/documents/${documentId}/Links/${linkId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, { LinkId }) => [
                { type: "Link", LinkId },
            ]
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
    })
})

export const {
    useCreateLinkMutation,
    useDeleteLinkMutation,
    useGetLinksByUserIdQuery,
} = LinkApi;