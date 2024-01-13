export interface Blog {
  id: number;
  userId: number;
  title: string;
  content: string;
  publicationDate: Date;
  categoryId: number;
  featuredImage: string;
  userFullName: string;
  commentCount: number;
}

export interface CreateBlog {
  userId: number;
  title: string;
  content: string;
  categoryId: number;
  featuredImage: string;
}

export interface Comment {
  id: number;
  userId: number;
  blogId: number;
  text: string;
  publicationDate: Date;
  userFullName: string;
}

export interface CreateComment {
  userId: number;
  blogId: number;
  text: string;
  publicationDate: Date;
}

export interface UpdateComment {
  id: number;
  userId: number;
  text: string;
  publicationDate: Date;
}

export interface Category {
  id: number;
  name: string;
}
