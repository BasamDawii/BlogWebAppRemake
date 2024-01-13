import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Category, CreateBlog} from "../../../app-services/app-models/BlogModels";
import {BlogService} from "../../../app-services/services/blog.service";
import {ClientMessageService} from "../../../app-services/services/client-message.service";
import {AccountService} from "../../../account-management/services/account.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-blog',
  templateUrl: './create-blog.component.html',
  styleUrls: ['./create-blog.component.scss'],
})
export class CreateBlogComponent implements OnInit {
  blogForm: FormGroup;
  categories: Category[] | undefined = [];
  userId!: number;

  constructor(
    private blogService: BlogService,
    private clientMessage: ClientMessageService,
    private accountService: AccountService,
    private router: Router
  ) {
    this.blogForm = new FormGroup({
      title: new FormControl('', Validators.required),
      content: new FormControl('', Validators.required),
      categoryId: new FormControl('', Validators.required),
      featuredImage: new FormControl('')
    });
  }

  ngOnInit(): void {
    this.blogService.getAllCategories().subscribe({
      next: (response) => this.categories = response.responseData,
      error: (err) => this.clientMessage.showError(err.error?.messageToClient)
    });

    this.accountService.whoAmI().subscribe({
      next: (response) => this.userId = response.responseData.id,
      error: (err) => this.clientMessage.showError(err.error?.messageToClient)
    });
  }

  onSubmit(): void {
    if (this.blogForm.valid) {
      const blogData: CreateBlog = {
        ...this.blogForm.value,
        userId: this.userId
      };

      this.blogService.createBlog(blogData).subscribe({
        next: (response) => {
          this.clientMessage.showInfo(response.messageToClient!);
          this.router.navigate(['/admin/my-blogs']);
        },
        error: (err) => this.clientMessage.showError(err.error?.messageToClient)
      });
    } else {
      this.clientMessage.showInfo('Please fill in all required fields');
    }
  }

}
