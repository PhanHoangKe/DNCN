
$(document).ready(function() {
    $('#CourseID').change(function() {
        var courseId = $(this).val();
        if (courseId) {
            $('#TeacherID').prop('disabled', false);
            $.ajax({
                url: '/Admin/Schedule/GetTeachersByCourse',
                type: 'GET',
                data: { courseId: courseId },
                success: function(response) {
                    var teacherSelect = $('#TeacherID');
                    teacherSelect.empty();
                    teacherSelect.append('<option value="">-- Chọn giảng viên --</option>');
                    $.each(response, function(index, item) {
                        teacherSelect.append($('<option></option>')
                            .val(item.teacherID)
                            .text(item.teacherName));
                    });
                },
                error: function() {
                    alert('Có lỗi xảy ra khi lấy thông tin giáo viên');
                }
            });
        } else {
            $('#TeacherID').prop('disabled', true);
            $('#TeacherID').empty();
            $('#TeacherID').append('<option value="">-- Chọn giảng viên --</option>');
        }
    });
});
