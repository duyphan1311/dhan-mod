using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.ModMenu
{
    public class ModMenuItemFunction : ModMenuItem
    {
        public Action Action { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Tiêu đề</param>
        /// <param name="description">Mô tả</param>
        /// <param name="action">Hàm được gọi khi được chọn, không có đối số và không trả về giá trị.</param>
        /// <param name="isDisabled">Trạng thái vô hiệu hóa</param>
        /// <param name="disabledReason">Lý do bị vô hiệu hóa, được thông báo khi ModMenuItem được chọn đang bị vô hiệu hóa.</param>
        public ModMenuItemFunction(string title, string description, Action action, bool isDisabled = false, string disabledReason = "") : base(title, description, isDisabled, disabledReason)
        {
            Action = action;
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItemFunction function &&
                   base.Equals(obj) &&
                   Title == function.Title &&
                   Description == function.Description &&
                   isDisabled == function.isDisabled &&
                   DisabledReason == function.DisabledReason;
        }

        public override int GetHashCode()
        {
            int hashCode = -2306474;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + isDisabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisabledReason);
            return hashCode;
        }
    }
}