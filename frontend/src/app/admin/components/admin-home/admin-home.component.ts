import { Component, OnInit } from '@angular/core';
import {Blog,Comment} from "../../../app-services/app-models/BlogModels";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {User} from "../../../app-services/app-models/AccountModels";
import {BlogService} from "../../../app-services/services/blog.service";
import {ClientMessageService} from "../../../app-services/services/client-message.service";
import {AccountService} from "../../../account-management/services/account.service";
import {AlertController} from "@ionic/angular";

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.scss'],
})
export class AdminHomeComponent implements OnInit {

  blogs: Blog[] = [];
  comments: Comment[] = [];
  commentForms: { [key: number]: FormGroup } = {};
  loggedUser!: User;

  constructor(
    private blogService: BlogService,
    private clientMessage: ClientMessageService,
    private accountService: AccountService,
    private alertController: AlertController
  ) { }

  ngOnInit(): void {
    this.accountService.whoAmI().subscribe({
      next: (response) => {
        if (response.responseData) {
          this.loggedUser = response.responseData;
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient);
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
          this.clientMessage.showInfo(response.messageToClient!);
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient);
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
        this.clientMessage.showInfo(err.error.messageToClient);
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
          this.clientMessage.showError(err.error?.messageToClient);
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
          this.clientMessage.showError(err.error?.messageToClient);
        }
      });
    } else {
      this.clientMessage.showError("Comment not found");
    }
  }

  deleteBlog(blogId: number): void {
    this.blogService.deleteBlogByAdmin(blogId).subscribe({
      next: (response) => {
        this.clientMessage.showInfo(response.messageToClient!);
        // Optionally, refresh the list of blogs
        this.refreshBlogs();
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient);
      }
    });
  }

  refreshBlogs(): void {
    this.blogService.getAllBlogsByUserId(this.loggedUser.id).subscribe({
      next: (response) => {
        if (response.responseData) {
          this.blogs = response.responseData;
        }
      },
      error: (err) => {
        this.clientMessage.showError(err.error?.messageToClient);
      }
    });
  }

  async confirmDelete(blogId: number) {
    const alert = await this.alertController.create({
      header: 'Confirm Delete',
      message: 'Do you really want to delete this blog?',
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
        },
        {
          text: 'Yes',
          handler: () => {
            this.deleteBlog(blogId);
          }
        }
      ]
    });

    await alert.present();
  }
}
