# 🖥️ نظام إدارة مكونات الحاسوب (Computer Components System) - Backend & MVC

نظام متكامل واحترافي لإدارة وتتبع قطع ومكونات أجهزة الحاسوب، التصنيفات، المواصفات الفنية، والمستخدمين. مبني باستخدام أحدث تقنيات **.NET 10** ومعمارية البرمجيات النظيفة **Clean Architecture**.

---

## 🌟 مميزات النظام (Key Features)

- 🧩 **إدارة المكونات (Components Management):** إضافة، تعديل، حذف، واستعراض قطع الهاردوير (معالجات، كروت شاشة، ذواكر، أقراص تخزين) مع الأسعار والموديل والصور.
- 📂 **إدارة التصنيفات (Categories):** تنظيم القطع ضمن تصنيفات محددة مع إحصاء عدد القطع في كل تصنيف تلقائياً.
- ⚙️ **المواصفات الفنية (Technical Specifications):** ربط مواصفات تفصيلية دقيقة بكل مكون (مثل السرعة، السعة، التردد، إلخ).
- 👥 **نظام إدارة المستخدمين والصلاحيات (Users & Roles):** إدارة حسابات المستخدمين والمدراء مع تشفير البيانات والتحكم في حالة الحساب (نشط / غير نشط).
- 📊 **لوحة تحكم إحصائية (Dashboard & Analytics):** إحصائيات حية حول إجمالي القطع، التصنيفات، القيمة المالية الإجمالية للمخزون، وتوزيع المنتجات.
- 🌐 **تكامل كامل (Cross-Platform API):** يوفر واجهات برمجية RESTful Web API تدعم CORS بالكامل لربط تطبيقات الموبايل والويب (Flutter).
- 🔐 **المصادقة والأمان:** دعم Cookie Authentication لواجهات الـ MVC، ومصادقة مستخدمي الـ API.

---

## 🏗️ المعمارية النظيفة (Clean Architecture Structure)

تم تنظيم المشروع بدقة وفق معمارية الطبقات المنفصلة لضمان سهولة الصيانة وقابلية التوسع والاختبار:

```text
ComputerComponentsSystem/
├── 📁 ComputerComponents.Domain/          # الكيانات الأساسية (Entities) وقواعد العمل النقية
│   └── Entities/ (Category, Component, Specification, User)
├── 📁 ComputerComponents.Application/     # حالات الاستخدام، الواجهات، ومنطق الأعمال (Interfaces & Services)
│   ├── Interfaces/ (IRepository, ICategoryService, ...)
│   └── Services/ (ComponentService, UserService, ...)
├── 📁 ComputerComponents.Infrastructure/  # قاعدة البيانات، الاتصال، وتنفيذ المستودعات (EF Core & Data)
│   ├── Data/ (AppDbContext)
│   ├── Repositories/ (Generic Repository & Specific Implementations)
│   └── Migrations/
├── 📁 ComputerComponents.MVC/             # واجهات المستخدم (Razor Views + Controllers + API Endpoints)
│   ├── Controllers/ (Home, Components, Categories, Users, ApiController, ...)
│   └── Views/ (لوحات الإدارة وعرض المنتجات وتعديلها)
└── 📁 ComputerComponents.Tests/           # اختبارات الوحدة الأوتوماتيكية (Unit Tests)
```

---

## 🛠️ التقنيات المستخدمة (Tech Stack)

- **Framework:** .NET 10.0 (C# 13)
- **Architecture:** Clean Architecture & Repository Pattern
- **ORM:** Entity Framework Core 10 (Code-First)
- **Database:** Microsoft SQL Server / LocalDB
- **Web Interface:** ASP.NET Core MVC & Razor Pages
- **Frontend Assets:** Bootstrap 5 (RTL Arabic Support), FontAwesome, Vanilla JS
- **API:** RESTful Web API (JSON Serializer with Cycle Detection)
- **Testing:** xUnit Framework

---

## 🚀 كيفية تشغيل المشروع (Getting Started)

### 1. المتطلبات الأساسية:
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) أو SQL Server LocalDB (مثبت تلقائياً مع Visual Studio).

### 2. التشغيل عبر سطر الأوامر (Terminal):
```powershell
cd ComputerComponents.MVC
dotnet run --urls "http://localhost:5067"
```

### 3. الروابط المتاحة محلياً:
- **بوابة الـ MVC ولوحة التحكم:** [http://localhost:5067](http://localhost:5067)
- **واجهات الـ API:** [http://localhost:5067/api](http://localhost:5067/api)

---

## 🔑 بيانات الدخول الافتراضية (Default Seeded Accounts)

تم تزويد قاعدة البيانات ببيانات أولية تلقائية فور التشغيل:

| الدور (Role) | البريد الإلكتروني (Email) | كلمة المرور (Password) |
| :--- | :--- | :--- |
| **مدير نظام (Admin)** | `admin@system.com` | `admin` |
| **مستخدم عادي (User)** | `ahmed@system.com` | `123` |

---

## 📡 أهم نقاط الـ API المتاحة لتطبيقات الموبايل (Endpoints)

- `GET /api/components` - استرجاع جميع المكونات
- `POST /api/components` - إضافة مكون جديد
- `GET /api/categories` - استرجاع كافة التصنيفات
- `GET /api/dashboard/stats` - إحصائيات لوحة التحكم
- `POST /api/users/login` - تسجيل الدخول والتحقق
