import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {ResponseDto} from "../app-models/ResponseModels";
import {Blog, Category, CreateBlog, CreateComment, UpdateComment,Comment} from "../app-models/BlogModels";
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  private readonly baseUrl = environment.apiUrl + '/api/blog/';
  constructor(private http: HttpClient) { }

  getAllBlogs(): Observable<ResponseDto<Blog[]>> {
    return this.http.get<ResponseDto<Blog[]>>(this.baseUrl + "blogs");
  }

  getAllBlogsByUserId(userId: number): Observable<ResponseDto<Blog[]>> {
    return this.http.get<ResponseDto<Blog[]>>(this.baseUrl + "blogs/user/" + userId, {withCredentials: true});
  }

  getCommentsByBlogId(blogId: number): Observable<ResponseDto<Comment[]>> {
    return this.http.get<ResponseDto<Comment[]>>(this.baseUrl + blogId + '/comments');
  }

  createComment(comment: CreateComment) : Observable<ResponseDto<CreateComment>> {
    return this.http.post<ResponseDto<CreateComment>>(environment.apiUrl + '/api/comment/create', comment, {withCredentials: true});
  }

  deleteComment(commentId: number): Observable<ResponseDto<any>> {
    return this.http.delete<ResponseDto<any>>(`${environment.apiUrl}/api/comment/delete/${commentId}`, { withCredentials: true });
  }
  updateComment(commentData: UpdateComment): Observable<ResponseDto<UpdateComment>> {
    return this.http.put<ResponseDto<UpdateComment>>(`${environment.apiUrl}/api/comment/update`, commentData, { withCredentials: true });
  }

  createBlog(blogData: CreateBlog): Observable<ResponseDto<Blog>> {
    return this.http.post<ResponseDto<Blog>>(this.baseUrl + 'create', blogData, { withCredentials: true });
  }

  getAllCategories(): Observable<ResponseDto<Category[]>> {
    return this.http.get<ResponseDto<Category[]>>(environment.apiUrl + '/api/category/categories', { withCredentials: true });
  }

  createCategory(categoryData: any): Observable<ResponseDto<Category>> {
    return this.http.post<ResponseDto<Category>>(environment.apiUrl + '/api/category/create', categoryData, { withCredentials: true });
  }

  updateCategory(categoryData: any): Observable<ResponseDto<Category>> {
    return this.http.put<ResponseDto<Category>>(environment.apiUrl + '/api/category/update', categoryData, { withCredentials: true });
  }

  deleteCategory(categoryId: number): Observable<ResponseDto<any>> {
    return this.http.delete<ResponseDto<any>>(environment.apiUrl + '/api/category/delete/' + categoryId, { withCredentials: true });
  }

  getCategoryById(categoryId: number): Observable<ResponseDto<Category>> {
    return this.http.get<ResponseDto<Category>>(environment.apiUrl + '/api/category/categories/' + categoryId, { withCredentials: true });
  }

  deleteBlog(blogId: number): Observable<ResponseDto<any>> {
    return this.http.delete<ResponseDto<any>>(`${this.baseUrl}delete-by-user/${blogId}`, { withCredentials: true });
  }
  deleteBlogByAdmin(blogId: number): Observable<ResponseDto<any>> {
    return this.http.delete<ResponseDto<any>>(`${this.baseUrl}delete-by-admin/${blogId}`, { withCredentials: true });
  }
}
