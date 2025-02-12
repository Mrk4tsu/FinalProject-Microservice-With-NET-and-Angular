import {Component} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DomSanitizer} from '@angular/platform-browser';
import {ImageCroppedEvent, ImageCropperComponent} from 'ngx-image-cropper';
import {AuthService} from '../../../auth/service/auth.service';
import {catchError, switchMap, tap, throwError} from 'rxjs';

@Component({
  selector: 'app-crop-image',
  imports: [
    ImageCropperComponent
  ],
  templateUrl: './crop-image.component.html',
  styleUrl: './crop-image.component.css'
})
export class CropImageComponent {
  imageChangedEvent: any = '';
  croppedImage: any = '';
  showCropper = false;
  showPreview = false;
  croppedImageFile: File | null = null;

  constructor(
    private sanitizer: DomSanitizer,
    private authService: AuthService
  ) {
  }

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
    this.showCropper = true;
    this.showPreview = false;
  }

  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = this.sanitizer.bypassSecurityTrustUrl(event.objectUrl!);

    // Chuyển đổi Blob thành File
    if (event.blob) {
      this.croppedImageFile = new File([event.blob], 'avatar.png', {type: event.blob.type});
    }
  }

  confirmCrop() {
    this.showCropper = false;
    this.showPreview = true;
  }

  recrop() {
    this.showCropper = true;
    this.showPreview = false;
  }

  saveImage() {
    if (this.croppedImageFile) {
      const formData = new FormData();
      formData.append('file', this.croppedImageFile);

      this.authService.updateAvatar(formData).subscribe({
        next: (res) => {
          this.showPreview = false;
        },
        error: (err) => {
          console.error('Lỗi trong quá trình xử lý:', err);
        }
      });
    }
  }

  cancel() {
    this.showCropper = false;
    this.showPreview = false;
    this.imageChangedEvent = ''; // Reset sự kiện chọn ảnh
    this.croppedImage = ''; // Reset ảnh đã crop
    this.croppedImageFile = null; // Reset file ảnh

    //reload lại trang
  }

  private dataURLtoFile(dataurl: string): File {
    const arr = dataurl.split(',');
    const mime = arr[0].match(/:(.*?);/)![1];
    const bstr = atob(arr[1]);
    let n = bstr.length;
    const u8arr = new Uint8Array(n);

    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }

    return new File([u8arr], 'avatar.png', {type: mime});
  }
}
