import {Component, OnInit} from '@angular/core';
import {Blog, Comment} from "../app-services/app-models/BlogModels";
import {BlogService} from "../app-services/services/blog.service";
import {ClientMessageService} from "../app-services/services/client-message.service";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements OnInit{

  blogs: Blog[] = [];
  comments: Comment[] = [];

  constructor(private blogService: BlogService, private clientMessage: ClientMessageService) { }

  ngOnInit(): void {
    this.blogService.getAllBlogs().subscribe({
      next: (response) => {
        if (response.responseData) {
          this.blogs = response.responseData;
        } else {
          this.clientMessage.showInfo(response.messageToClient!)
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient)
      }
    });
  }

  loadComments(blogId: number): void {
    this.blogService.getCommentsByBlogId(blogId).subscribe({
      next: (response) => {
        if (response.responseData) {
          this.comments = response.responseData;
        } else {
          this.clientMessage.showInfo(response.messageToClient!)
        }
      },
      error: (err) => {
        this.clientMessage.showInfo(err.error?.messageToClient)
      }
    });
  }
}
