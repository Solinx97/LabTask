import type { CommentModel } from '../types/CommentModel';
import { DocumentApi } from './Document.api';

export const CommentApi = DocumentApi.injectEndpoints({
    endpoints: builder => ({
        createComment: builder.mutation<CommentModel, CommentModel>({
            query: comment => ({
                body: comment,
                url: '/Comment',
                method: 'POST'
            }),
            async onQueryStarted(comment, { dispatch, queryFulfilled }) {
                const patches = [
                    dispatch(
                        CommentApi.util.updateQueryData(
                            'getCommentsByDocumentId',
                            { documentId: comment.documentId },
                            draft => {
                                draft.unshift(comment);
                            }
                        )
                    ),
                ];

                try {
                    const { data: created } = await queryFulfilled;

                    dispatch(
                        CommentApi.util.updateQueryData(
                            'getCommentsByDocumentId',
                            { documentId: created.documentId },
                            draft => {
                                const index = draft.findIndex(l => l.id === comment.id);
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
        updateComment: builder.mutation<void, CommentModel>({
            query: comment => ({
                body: comment,
                url: `/Comment/${comment.id}`,
                method: 'PATCH'
            }),
            async onQueryStarted(comment, { dispatch, queryFulfilled }) {
                const patchResult = dispatch(
                    CommentApi.util.updateQueryData(
                        'getCommentsByDocumentId',
                        { documentId: comment.documentId },
                        draft => {
                            const com = draft.find(c => c.id === comment.id);
                            if (com) {
                                Object.assign(com, comment);
                            }
                        }
                    )
                );
                try {
                    await queryFulfilled;
                } catch {
                    patchResult.undo();
                }
            }
        }),
        deleteComment: builder.mutation<void, { documentId: string, commentId: string }>({
            query: ({ documentId, commentId }) => ({
                url: `/Comment/documents/${documentId}/comments/${commentId}`,
                method: 'DELETE'
            }),
            async onQueryStarted({ documentId, commentId }, { dispatch, queryFulfilled }) {
                const patches = [
                    dispatch(
                        CommentApi.util.updateQueryData(
                            'getCommentsByDocumentId',
                            { documentId },
                            draft => {
                                const index = draft.findIndex(d => d.id === commentId);
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
        getAllComments: builder.query<CommentModel[], void>({
            query: () => "/Comment",
            providesTags: result =>
                result
                    ? [
                        ...result.map(docuemnt => ({ type: 'Comment' as const, id: docuemnt.id })),
                        { type: 'Comment', id: 'LIST' },
                    ]
                    : [{ type: 'Comment', id: 'LIST' }],
        }),
        getCommentsByDocumentId: builder.query<CommentModel[], { documentId: string, page: number, pageSize: number }>({
            query: ({ documentId, page, pageSize }) => `/Comment/getByDocumentId/${documentId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.documentId}`,
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
                        ...result.map(comment => ({ type: 'Comment' as const, id: comment.id })),
                        { type: 'Comment', id: 'LIST' },
                    ]
                    : [{ type: 'Comment', id: 'LIST' }],
        }),
        getCommentsByUserId: builder.query<CommentModel[], string>({
            query: userId => `/Comment/getByUserId/${userId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(comment => ({ type: 'Comment' as const, id: comment.id })),
                        { type: 'Comment', id: 'LIST' },
                    ]
                    : [{ type: 'Comment', id: 'LIST' }],
        }),
    })
})

export const {
    useCreateCommentMutation,
    useUpdateCommentMutation,
    useDeleteCommentMutation,
    useGetAllCommentsQuery,
    useGetCommentsByDocumentIdQuery,
    useGetCommentsByUserIdQuery,
} = CommentApi;