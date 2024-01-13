import { Component, OnInit } from '@angular/core';
import {Blog,Comment} from "../../../app-services/app-models/BlogModels";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {User} from "../../../app-services/app-models/AccountModels";
import {BlogService} from "../../../app-services/services/blog.service";
import {ClientMessageService} from "../../../app-services/services/client-message.service";
import {AccountService} from "../../../account-management/services/account.service";

@Component({
  selector: 'app-user-home',
  templateUrl: './user-home.component.html',
  styleUrls: ['./user-home.component.scss'],
})
export class UserHomeComponent  implements OnInit {

  blogs: Blog[] = [];
  comments: Comment[] = [];
  commentForms: { [key: number]: FormGroup } = {};
  loggedUser!: User;

  constructor(private blogService: BlogService, private clientMessage: ClientMessageService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.whoAmI().subscribe({
      next: (response) => {
        if (response.responseData) {
          this.loggedUser = response.responseData;
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.message);
      }
    });

    this.blogService.getAllBlogs().subscribe({
      next: (response) => {
        if (response.responseData) {
          this.blogs = response.responseData;
          this.blogs.forEach(blog => {
            this.commentForms[blog.id] = new FormGroup({
              userId: new FormControl(this.loggedUser?.id, [Validators.required]),
              blogId: new FormControl(blog.id, [Validators.required]),
              text: new FormControl('', [Validators.required]),
              publicationDate: new FormControl(new Date())
            });
          });
        } else {
          this.clientMessage.showInfo("No blogs available");
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.message);
      }
    });
  }

  loadComments(blogId: number): void {
    this.blogService.getCommentsByBlogId(blogId).subscribe({
      next: (response) => {
        if (response.responseData) {
          this.comments = response.responseData.map(comment => ({
            ...comment,
            blogId: blogId
          }));
        } else {
          this.clientMessage.showInfo("No comments available");
        }
      },
      error: (err) => {
        this.clientMessage.showInfo("No comments available");
      }
    });
  }


  submitComment(blogId: number): void {
    const commentForm = this.commentForms[blogId];
    if (commentForm.valid) {
      this.blogService.createComment(commentForm.value).subscribe({
        next: (response) => {
          this.clientMessage.showInfo("Comment successfully added");
          this.loadComments(blogId);
          commentForm.get('text')?.reset();
        },
        error: (err) => {
          this.clientMessage.showError(err.error?.message);
        }
      });
    } else {
      this.clientMessage.showInfo("Please fill in all required fields");
    }
  }

  deleteComment(commentId: number): void {
    const comment = this.comments.find(c => c.id === commentId);
    if (comment) {
      this.blogService.deleteComment(commentId).subscribe({
        next: (response) => {
          this.clientMessage.showInfo(response.messageToClient!);
          this.loadComments(comment.blogId);
        },
        error: (err) => {
          this.clientMessage.showError(err.error?.message);
        }
      });
    } else {
      this.clientMessage.showError("Comment not found");
    }
  }


}
